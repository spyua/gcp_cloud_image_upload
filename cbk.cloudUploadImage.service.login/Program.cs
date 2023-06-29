using cbk.cloudUploadImage.Infrastructure;
using cbk.cloudUploadImage.Infrastructure.Config;
using cbk.cloudUploadImage.Infrastructure.Help.Certified;
using cbk.cloudUploadImage.Infrastructure.Help.DBConnection;
using cbk.cloudUploadImage.Infrastructure.Help.DBConnection.Model;
using cbk.cloudUploadImage.Infrastructure.Help.Internet;
using cbk.cloudUploadImage.Infrastructure.Repository;
using cbk.cloudUploadImage.service.login.Service;
using Google.Api;
using JWT.Algorithms;
using JWT.Extensions.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var environmentConfig = new EnvironmentConfig(useMock:true);

// Infrstructure Injection Setting
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

// System Config Setting
builder.Services.Configure<JwtSettingsOptions>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddOptions<JwtSettingsOptions>("JwtSettings");
builder.Services.AddSingleton<JwtHelpers>();
builder.Services.AddSingleton<IAlgorithmFactory>(new DelegateAlgorithmFactory(new HMACSHA256Algorithm()));


// Add services to the container.
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddControllers();

// Middleware invaild JWT token setting

/*
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:Issuer")))
        };
    });
*/

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtAuthenticationDefaults.AuthenticationScheme;

}) .AddJwt(options =>
    {
        // 這段要抽換....去拿KMS的Key
        options.Keys = new string[] { builder.Configuration.GetValue<string>("JwtSettings:SignKey") };
        options.VerifySignature = true;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Login API", Version = "v1" });

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
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Middleware invaild JWT token setting
app.UseAuthentication();
app.UseAuthorization();

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

app.MapControllers();

app.Run();
