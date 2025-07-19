using AgriNaviApi.Application.Extensions;
using AgriNaviApi.Infrastructure.Persistence.Contexts;
using AgriNaviApi.Shared.Exceptions;
using AgriNaviApi.Shared.Extensions;
using AgriNaviApi.Shared.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 1. リソースファイルの場所を指定（resx は /Resources フォルダ下に配置）
builder.Services.AddLocalization(opts => opts.ResourcesPath = "Resources");

builder.Services
    .AddControllers()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(type);
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// appsettings.jsonからデフォルトのDB接続文字列を取得
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var mySqlVersionString = builder.Configuration.GetValue<string>("DatabaseOptions:MySqlVersion");

// MySQLバージョン変換
if (!Version.TryParse(mySqlVersionString, out var mySqlVersion))
{
    throw new InvalidOperationException($"アプリ設定のMySQLバージョン指定 が不正です: '{mySqlVersionString}'");
}
var serverVersion = new MySqlServerVersion(mySqlVersion);

// appsettings.json から PathBase を取得
var pathBase = builder.Configuration.GetValue<string>("PathBase") ?? "/agritool/api";

// DbContextのサービスの登録
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, serverVersion));

// Shared層のサービスの登録
builder.Services.AddSharedServices();
// Application層のサービスの登録
builder.Services.AddApplicationServices();

// Swagger UI で使う JSON のエンドポイントもUsePathBaseを含めたパスにする
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "AgriNavi API", Version = "v1" });

    // PathBase を Swagger に通知する
    options.AddServer(new OpenApiServer
    {
        Url = pathBase
    });
});

var app = builder.Build();

// AgriNavi共通パス
app.UsePathBase(pathBase);

// グローバル例外ハンドラを登録し、キャッチされなかった例外をここで処理する  
app.UseExceptionHandler(errorApp =>
{
    // errorAppに例外判定処理を渡す
    errorApp.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        // 例外情報を取得
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = feature?.Error;

        // 例外をログに記録
        logger.LogError(ex, "キャッチされなかった例外が発生しました");

        // DIコンテナから.resx などのリソースファイルを読み込むためのファクトリを取得
        var factory = context.RequestServices
                         .GetRequiredService<IStringLocalizerFactory>();

        // ILocalizedExceptionを持つ例外かを判定
        if (ex is ILocalizedException lex)
        {
            // どのresxファイルを使うかを取得
            var localizer = factory.Create(lex.ResourceType);
            // メッセージを取得
            var message = localizer[lex.ResourceKey, lex.Args];

            // 例外毎にステータスコードを振り分ける
            var statusCode = ex switch
            {
                DuplicateEntityException => StatusCodes.Status409Conflict,
                EntityNotFoundException => StatusCodes.Status404NotFound,
                PasswordHashingException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status400BadRequest
            };

            // 振り分けたステータスコードをセット
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsJsonAsync(new
            {
                error = ex.GetType().Name,
                message = message.Value
            });
            return;
        }

        // それ以外は 500  
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json; charset=utf-8";
        await context.Response.WriteAsJsonAsync(new
        {
            error = "InternalServerError",
            message = "予期しないエラーが発生しました。"
        });
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
