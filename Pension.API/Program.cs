using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Hangfire;
using Pension.Infrastructure.Repositories;
using Pension.Domain.Repositories;
using Pension.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Pension.Application.Services.Auth;
using Hangfire.SqlServer;




var builder = WebApplication.CreateBuilder(args);


// Add JWT Authentication
var jwtKey = builder.Configuration["JwtSettings:Secret"];
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
var jwtAudience = builder.Configuration["JwtSettings:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddSingleton<JwtTokenService>();

builder.Services.AddAuthorization();



// Register the DbContext with SQL Server
builder.Services.AddDbContext<PensionsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PensionsDb"))
);

// Add Hangfire configuration with custom retry and poll interval
builder.Services.AddHangfire(config => config.UseSqlServerStorage(builder.Configuration.GetConnectionString("PensionsDb"), new SqlServerStorageOptions
{
    InvisibilityTimeout = TimeSpan.FromMinutes(5),  // Custom retry interval
    QueuePollInterval = TimeSpan.FromSeconds(30)   // How often Hangfire checks for jobs
}));

// Add Hangfire server
builder.Services.AddHangfireServer();

// Add other services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Pensions Management API",
        Version = "v1"
    });
});

builder.Services.AddScoped<IMemberRepository, MemberRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.UseHangfireDashboard();

app.UseMiddleware<GlobalExceptionMiddleware>();


app.Run();
