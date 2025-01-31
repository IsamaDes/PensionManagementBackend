using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Hangfire;
using Pension.Infrastructure.Repositories;
using Pension.Domain.Repositories;
using Pension.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

// Register the DbContext with SQL Server
builder.Services.AddDbContext<PensionsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PensionsDb"))
);

builder.Services.AddHangfire(config => config.UseSqlServerStorage(builder.Configuration.GetConnectionString("PensionsDb")));
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
