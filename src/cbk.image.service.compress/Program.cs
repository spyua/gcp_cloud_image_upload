using cbk.image.Infrastructure.CloudRunEnviroment;
using cbk.image.Infrastructure.CloudRunEnviroment.Storage;
using cbk.image.Infrastructure.Database.DBConnection.Model;
using cbk.image.Infrastructure.Database.DBConnection;
using Microsoft.EntityFrameworkCore;
using cbk.image.Infrastructure.Database;
using cbk.image.Infrastructure.CloudRunEnviroment.DB;
using cbk.image.Infrastructure.CloudRunEnviroment.KMSEncryption;
using cbk.image.service.compress.Service;
using cbk.image.Infrastructure.Repository;
using cbk.cloud.gcp.serviceProvider.CloudRun.EnviromentConfig;
using cbk.image.Web.Middleware;
using cbk.cloud.serviceProvider.Storage;

var builder = WebApplication.CreateBuilder(args);
// Configure the application.
builder.Configuration["ASPNETCORE_URLS"] = $"http://*:{Environment.GetEnvironmentVariable("PORT") ?? "8080"}";
// 需設置ASPNETCORE_ENVIRONMENT 環境變數 (你可以設在Dockerfile上，若Cloud Run，則設在Cloud Run上)
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton(provider => new MockEncryptionConfigFactory().Create());
    builder.Services.AddSingleton(provider => new MockDBConfigFactory().Create());
    builder.Services.AddSingleton(provider => new StorageEnvironmentConfig(useMock: true));
}
else
{
    builder.Services.AddSingleton(provider => new EncryptionEnvironmentConfigFactory().Create());
    builder.Services.AddSingleton(provider => new DBEnvironmentConfigFactory().Create());
    builder.Services.AddSingleton(provider => new StorageEnvironmentConfig(useMock: false));
}
builder.Services.AddSingleton<IEnvironmentConfig, EnvironmentConfig>();

// DB Inject Setting
builder.Services.AddSingleton(provider =>
{

    var environmentConfig = provider.GetRequiredService<IEnvironmentConfig>();
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
    return connectionSetting;
});
builder.Services.AddDbContext<DBContext>((serviceProvider, options) => {
    IDBConnectionBuilder connectionBuilder = new NpgsqlConnectionBuilder<DBConnectionSetting>();
    var connectionSetting = serviceProvider.GetService<DBConnectionSetting>();
    var connectionString = connectionBuilder.BuildConnectionString(connectionSetting, true);
    options.UseNpgsql(connectionString);
});

// Add services to the container.
builder.Services.AddSingleton<IStorageService, GoogleStorageService>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IImageCompressorService, ImageCompressorService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.Run();
