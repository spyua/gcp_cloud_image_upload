using cbk.cloudUploadImage.Infrastructure;
using cbk.cloudUploadImage.Infrastructure.Help.DBConnection;
using cbk.cloudUploadImage.Infrastructure.Help.DBConnection.Model;
using cbk.cloudUploadImage.Infrastructure.Repository;
using cbk.cloudUploadImage.service.login.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB Setting (Wait Clean)
string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
string certFolder = "Certificate";
string clientCertFile = "client-cert.pem";
string clientKeyFile = "client-key.pem";
string serverCaFile = "server-ca.pem";

IDBConnectionBuilder connectionBuilder = new NpgsqlConnectionBuilder<DBConnectionSetting>();
var connectionSetting = new DBConnectionSetting()
{
    InstanceName = "35.229.242.171",
    DatabaseName = "postgres",
    UserName = "cbk_testing",
    Password = "cbktesting",
    SeverCertificatePath = Path.Combine(baseDirectory, certFolder, serverCaFile),
    ClientCertificatePath = Path.Combine(baseDirectory, certFolder, clientCertFile),
    ClientCertificateKeyPath = Path.Combine(baseDirectory, certFolder, clientKeyFile)
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
