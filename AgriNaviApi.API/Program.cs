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
// 1. ���\�[�X�t�@�C���̏ꏊ���w��iresx �� /Resources �t�H���_���ɔz�u�j
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

// appsettings.json����f�t�H���g��DB�ڑ���������擾
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var mySqlVersionString = builder.Configuration.GetValue<string>("DatabaseOptions:MySqlVersion");

// MySQL�o�[�W�����ϊ�
if (!Version.TryParse(mySqlVersionString, out var mySqlVersion))
{
    throw new InvalidOperationException($"�A�v���ݒ��MySQL�o�[�W�����w�� ���s���ł�: '{mySqlVersionString}'");
}
var serverVersion = new MySqlServerVersion(mySqlVersion);

// appsettings.json ���� PathBase ���擾
var pathBase = builder.Configuration.GetValue<string>("PathBase") ?? "/agritool/api";

// DbContext�̃T�[�r�X�̓o�^
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, serverVersion));

// Shared�w�̃T�[�r�X�̓o�^
builder.Services.AddSharedServices();
// Application�w�̃T�[�r�X�̓o�^
builder.Services.AddApplicationServices();

// Swagger UI �Ŏg�� JSON �̃G���h�|�C���g��UsePathBase���܂߂��p�X�ɂ���
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "AgriNavi API", Version = "v1" });

    // PathBase �� Swagger �ɒʒm����
    options.AddServer(new OpenApiServer
    {
        Url = pathBase
    });
});

var app = builder.Build();

// AgriNavi���ʃp�X
app.UsePathBase(pathBase);

// �O���[�o����O�n���h����o�^���A�L���b�`����Ȃ�������O�������ŏ�������  
app.UseExceptionHandler(errorApp =>
{
    // errorApp�ɗ�O���菈����n��
    errorApp.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        // ��O�����擾
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = feature?.Error;

        // ��O�����O�ɋL�^
        logger.LogError(ex, "�L���b�`����Ȃ�������O���������܂���");

        // DI�R���e�i����.resx �Ȃǂ̃��\�[�X�t�@�C����ǂݍ��ނ��߂̃t�@�N�g�����擾
        var factory = context.RequestServices
                         .GetRequiredService<IStringLocalizerFactory>();

        // ILocalizedException������O���𔻒�
        if (ex is ILocalizedException lex)
        {
            // �ǂ�resx�t�@�C�����g�������擾
            var localizer = factory.Create(lex.ResourceType);
            // ���b�Z�[�W���擾
            var message = localizer[lex.ResourceKey, lex.Args];

            // ��O���ɃX�e�[�^�X�R�[�h��U�蕪����
            var statusCode = ex switch
            {
                DuplicateEntityException => StatusCodes.Status409Conflict,
                EntityNotFoundException => StatusCodes.Status404NotFound,
                PasswordHashingException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status400BadRequest
            };

            // �U�蕪�����X�e�[�^�X�R�[�h���Z�b�g
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsJsonAsync(new
            {
                error = ex.GetType().Name,
                message = message.Value
            });
            return;
        }

        // ����ȊO�� 500  
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json; charset=utf-8";
        await context.Response.WriteAsJsonAsync(new
        {
            error = "InternalServerError",
            message = "�\�����Ȃ��G���[���������܂����B"
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
