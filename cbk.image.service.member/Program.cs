using cbk.cloud.serviceProvider.CloudRun.EnviromentConfig;
using cbk.cloud.serviceProvider.KMS;
using cbk.image.Infrastructure.Config;
using cbk.image.Infrastructure.Config.KMSEncryption;
using cbk.image.Infrastructure.Database;
using cbk.image.Infrastructure.Database.DBConnection;
using cbk.image.Infrastructure.Database.DBConnection.Model;
using cbk.image.Infrastructure.Middleware;
using cbk.image.Infrastructure.Repository;
using cbk.image.Infrastructure.Security.Jwt;
using cbk.image.service.member.Service;
using JWT.Algorithms;
using JWT.Extensions.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var environmentConfig = new EnvironmentConfig(useMock:true);
builder.Configuration["ASPNETCORE_URLS"] = $"http://*:{Environment.GetEnvironmentVariable("PORT") ?? "8080"}";


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


builder.Services.AddSingleton<IEncryptionEnvironmentConfig>(provider => new EncryptionEnvironmentConfig(true));
builder.Services.AddSingleton<IKmsService, GoogleKmsService>();


builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddOptions<JwtSettings>("JwtSettings");
builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddSingleton<IAlgorithmFactory>(new DelegateAlgorithmFactory(new HMACSHA256Algorithm()));

// Add services to the container.
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddControllers();


// Middleware invaild JWT token setting

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtAuthenticationDefaults.AuthenticationScheme;

}) .AddJwt(options =>
    {
        // 這段要抽換....去拿Security Manager Key
        options.Keys = new string[] { builder.Configuration.GetValue<string>("JwtSettings:TokenSecret") };
        options.VerifySignature = true;
    });

/* 保留 Net原生寫法
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false, // 如果你的 JWT 包含 "aud" claim，那麼你需要將這個設定為 true
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SignKey"]))
        };
    });
*/
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
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

/*
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/

// app.UseHttpsRedirection();
// Middleware invaild JWT token setting
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
app.Run();
