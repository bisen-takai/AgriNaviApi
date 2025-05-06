using AgriNaviApi.Application.Extensions;
using AgriNaviApi.Common.Extensions;
using AgriNaviApi.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// appsettings.json����f�t�H���g��DB�ڑ���������擾
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var mySqlVersionString = builder.Configuration.GetValue<string>("DatabaseOptions:MySqlVersion");

// MySQL�o�[�W�����ϊ�
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

    // PathBase �� Swagger �ɒʒm����
    options.AddServer(new OpenApiServer
    {
        Url = "/agritool"
    });
});
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, serverVersion));

// Common�w�̃T�[�r�X�̓o�^
builder.Services.AddCommonServices();
// Application�w�̃T�[�r�X�̓o�^
builder.Services.AddApplicationServices();

var app = builder.Build();

// AgriNavi���ʃp�X
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
