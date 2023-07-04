using cbk.image.Infrastructure.Config;
using cbk.image.Infrastructure.Database;
using cbk.image.Infrastructure.Database.DBConnection.Model;
using cbk.image.Infrastructure.Database.DBConnection;
using Microsoft.EntityFrameworkCore;
using cbk.image.Infrastructure.Security.Jwt;
using JWT.Algorithms;
using JWT.Extensions.AspNetCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using cbk.image.Infrastructure.Middleware;
using cbk.cloud.serviceProvider.Storage;
using cbk.image.service.upload.Service;
using cbk.image.Infrastructure.Repository;
using cbk.image.Infrastructure.Config.Storage;

var builder = WebApplication.CreateBuilder(args);
var environmentConfig = new EnvironmentConfig(useMock: true);
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

builder.Services.AddSingleton<IStorageService, GoogleStorageService>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddOptions<JwtSettings>("JwtSettings");
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IAlgorithmFactory>(new DelegateAlgorithmFactory(new HMACSHA256Algorithm()));

builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtAuthenticationDefaults.AuthenticationScheme;

}).AddJwt(options =>
{
    // 這段要抽換....去拿Security Manager Key
    options.Keys = new string[] { builder.Configuration.GetValue<string>("JwtSettings:TokenSecret") };
    options.VerifySignature = true;
});
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
