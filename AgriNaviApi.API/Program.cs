using AgriNaviApi.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, serverVersion));

var app = builder.Build();

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
