using cbk.image.Infrastructure.Config;
using cbk.image.Infrastructure.Config.Storage;
using cbk.image.Infrastructure.Database.DBConnection.Model;
using cbk.image.Infrastructure.Database.DBConnection;
using Microsoft.EntityFrameworkCore;
using cbk.image.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);
var environmentConfig = new EnvironmentConfig(useMock: true);

// «Ý³B²z
builder.Configuration["ASPNETCORE_URLS"] = $"http://*:{Environment.GetEnvironmentVariable("PORT") ?? "8080"}";

builder.Services.AddSingleton(provider => new StorageEnvironmentConfig(true));

IDBConnectionBuilder connectionBuilder = new NpgsqlConnectionBuilder<DBConnectionSetting>();
var connectionSetting = new DBConnectionSetting()
{
    InstanceName = environmentConfig.DbConfig.InstanceName,
    DatabaseName = environmentConfig.DbConfig.DatabaseName,
    UserName = environmentConfig.DbConfig.UserName,
    Password = environmentConfig.DbConfig.Password,
    SeverCertificatePath = environmentConfig.DbConfig.SeverCertificatePath,
    ClientCertificatePath = environmentConfig.DbConfig.ClientCertificatePath,
    ClientCertificateKeyPath = environmentConfig.DbConfig.ClientCertificateKeyPath,
};
var connectionString = connectionBuilder.BuildConnectionString(connectionSetting, true);
builder.Services.AddDbContext<DBContext>(options =>
    options.UseNpgsql(connectionString));




// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
