using cbk.cloudUploadImage.Infrastructure;
using cbk.cloudUploadImage.Infrastructure.Config;
using cbk.cloudUploadImage.Infrastructure.Help.DBConnection;
using cbk.cloudUploadImage.Infrastructure.Help.DBConnection.Model;
using cbk.cloudUploadImage.Infrastructure.Repository;
using cbk.cloudUploadImage.service.login.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var environmentConfig = new EnvironmentConfig(useMock:true);

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
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();

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
