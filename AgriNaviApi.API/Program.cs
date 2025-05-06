using AgriNaviApi.Application.Extensions;
using AgriNaviApi.Common.Extensions;
using AgriNaviApi.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// appsettings.jsonからデフォルトのDB接続文字列を取得
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var mySqlVersionString = builder.Configuration.GetValue<string>("DatabaseOptions:MySqlVersion");

// MySQLバージョン変換
Version mySqlVersion;
mySqlVersion = Version.Parse(mySqlVersionString);
var serverVersion = new MySqlServerVersion(mySqlVersion);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "AgriNavi API", Version = "v1" });

    // PathBase を Swagger に通知する
    options.AddServer(new OpenApiServer
    {
        Url = "/agritool"
    });
});
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, serverVersion));

// Common層のサービスの登録
builder.Services.AddCommonServices();
// Application層のサービスの登録
builder.Services.AddApplicationServices();

var app = builder.Build();

// AgriNavi共通パス
app.UsePathBase("/agritool");

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
