using cbk.image.Infrastructure.Database;
using cbk.image.Infrastructure.Database.DBConnection.Model;
using cbk.image.Infrastructure.Database.DBConnection;
using Microsoft.EntityFrameworkCore;
using cbk.image.Infrastructure.Security.Jwt;
using JWT.Algorithms;
using JWT.Extensions.AspNetCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using cbk.cloud.serviceProvider.Storage;
using cbk.image.service.upload.Service;
using cbk.image.Infrastructure.Repository;
using cbk.image.Infrastructure.CloudRunEnviroment.Storage;
using cbk.image.Infrastructure.CloudRunEnviroment.DB;
using cbk.image.Infrastructure.CloudRunEnviroment.KMSEncryption;
using cbk.cloud.gcp.serviceProvider.CloudRun.EnviromentConfig;
using cbk.image.Infrastructure.CloudRunEnviroment.IAM;
using cbk.image.Web.Middleware;
using cbk.image.Infrastructure.CloudRunEnviroment;

var builder = WebApplication.CreateBuilder(args);

// Configure the application.
builder.Configuration["ASPNETCORE_URLS"] = $"http://*:{Environment.GetEnvironmentVariable("PORT") ?? "8080"}";

// 需設置ASPNETCORE_ENVIRONMENT 環境變數 (你可以設在Dockerfile上，若Cloud Run，則設在Cloud Run上)
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton(provider => new MockEncryptionConfigFactory().Create());
    builder.Services.AddSingleton(provider => new MockDBConfigFactory().Create());
    builder.Services.AddSingleton(provider => new StorageEnvironmentConfig(useMock:true));
    builder.Services.AddSingleton(provider => new AccountServiceCredentialConfig(useMock: true));

}
else
{
    builder.Services.AddSingleton(provider => new EncryptionEnvironmentConfigFactory().Create());
    builder.Services.AddSingleton(provider => new DBEnvironmentConfigFactory().Create());
    builder.Services.AddSingleton(provider => new StorageEnvironmentConfig(useMock:false));
    builder.Services.AddSingleton(provider => new AccountServiceCredentialConfig(useMock: false));
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
    if (connectionSetting == null)
        throw new Exception("DBConnectionSetting is null, You don't setting the cloud run EnvironmentVariable");
    var connectionString = connectionBuilder.BuildConnectionString(connectionSetting, true);
    options.UseNpgsql(connectionString);
});

// JWT Token Inject Setting
builder.Services.AddSingleton(options =>
{
    var jwtSettings = new JwtSettings();
    jwtSettings.Issuer = builder.Configuration["JwtSettings:Issuer"];
    jwtSettings.ExpiredDay = int.Parse(builder.Configuration["JwtSettings:ExpiredDay"]);

    if (builder.Environment.IsDevelopment())
    {
        jwtSettings.TokenSecret = builder.Configuration["JwtSettings:TokenFakeSecret"];
    }
    else
    {
        var tokenSecret = Environment.GetEnvironmentVariable("SECRET_TOKEN_KEY");
        if (string.IsNullOrEmpty(tokenSecret))
            throw new Exception("TokenSecret is null or empty, You don't setting the cloud run EnvironmentVariable");
        jwtSettings.TokenSecret = tokenSecret;
    }

    return jwtSettings;
});
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IAlgorithmFactory>(new DelegateAlgorithmFactory(new HMACSHA256Algorithm()));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtAuthenticationDefaults.AuthenticationScheme;

}).AddJwt(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.Keys = new string[] { builder.Configuration.GetValue<string>("JwtSettings:TokenFakeSecret") };
    }
    else
    {
        var tokenSecret = Environment.GetEnvironmentVariable("SECRET_TOKEN_KEY");
        if (string.IsNullOrEmpty(tokenSecret))
            throw new Exception("TokenSecret is null or empty, You don't setting the cloud run EnvironmentVariable");
        options.Keys = new string[] { tokenSecret };
    }
    options.VerifySignature = true;
});


builder.Services.AddSingleton<IStorageService, GoogleStorageService>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Image Upload API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});
var app = builder.Build();

// Middleware Setting
if (builder.Environment.IsDevelopment())
{
    app.Logger.LogInformation("Using development environment settings.");
}
else
{
    app.Logger.LogInformation("Using production environment settings.");
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        if (ex is SecurityTokenException)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid token.");
        }
        else
        {
            throw;
        }
    }
});
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.UseMiddleware<ValidateImageFileTypeMiddleware>();
app.Run();
