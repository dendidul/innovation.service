using AHM.Logger.Serilog;
using AHM.OpenAPI.Swagger;
using AHM.OpenTelemetry.Jaeger;
using Core.Application;
using Infrastructure.ExternalService;
using Infrastructure.Persistence;
using AHM.Authentication.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
// Add services to the container.
builder.Configuration.AddJsonFile($"ConfigMap/central-config.json", optional: true).AddEnvironmentVariables();
builder.Configuration.AddJsonFile($"ConfigMap/central-secret.json", optional: true).AddEnvironmentVariables();
builder.Configuration.AddJsonFile($"ConfigMap/service-config.json", optional: true).AddEnvironmentVariables();

builder.Services.AddAhmJwtAuthentication(builder.Configuration);
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterApplicationServices(builder.Configuration);
builder.Services.RegisterPersistenceServices(builder.Configuration);
builder.Services.RegisterExternalServices(builder.Configuration);
builder.Services.AddJaegerTracing(builder.Configuration);
builder.Services.AddAhmSwagger(builder.Configuration);
builder.Host.RegisterSerilog(builder.Configuration);
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
