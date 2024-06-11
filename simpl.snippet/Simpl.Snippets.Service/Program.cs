using FluentValidation;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using Simpl.Snippets.Service.DataAccess.Abstract;
using Simpl.Snippets.Service.DataAccess.Models;
using Simpl.Snippets.Service.DataAccess.Repositories;
using Simpl.Snippets.Service.Domain.Authorization.Extensions;
using Simpl.Snippets.Service.Domain.Authorization.Services;
using Simpl.Snippets.Service.Domain.CodeRunner;
using Simpl.Snippets.Service.Domain.CodeShare;
using Simpl.Snippets.Service.Domain.CodeShare.Abstract;
using Simpl.Snippets.Service.Domain.CodeShare.Services;
using Simpl.Snippets.Service.Middlewares.Extensions;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();
var configuration = builder.Configuration;

var logger = LogManager.GetCurrentClassLogger();

try
{
    builder.Logging.ClearProviders();
    builder.Host.UseNLog(new NLogAspNetCoreOptions { RemoveLoggerFactoryFilter = false });

    builder.Services.AddHealthChecks();

    builder.Services.Configure<SnippetDatabaseSettings>(configuration.GetSection("SnippetDatabaseSettings"));
    builder.Services.AddSignalR();
    builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration["SnippetRedisSettings:ConnectionString"]));
    builder.Services.AddTransient<IRedisService, RedisService>();
    builder.Services.AddScoped<ISnippetRepository, MongoDbSnippetRepository>();
    builder.Services.AddScoped<IAuthorRepository, MongoDbAuthorRepository>();

    builder.Services.AddMediatR(c => c
        .RegisterServicesFromAssemblyContaining<Program>()
        .AddOpenRequestPreProcessor(typeof(AuthPreProcessorById<>))
        .AddOpenRequestPreProcessor(typeof(AuthPreProcessorByDirection<>))
        );

    builder.Services.AddValidatorsFromAssemblyContaining<Program>();

    builder.Services.AddControllers(o => o.RequireGlobalAuthentication())
        .AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

    builder.Services.AddKeycloakAuthorization(configuration, new NLogLoggerProvider().CreateLogger(nameof(Program)));

    builder.Services.AddSwaggerGen(opt =>
    {
        var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        opt.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    });

    builder.Services.AddCodeRunnerFactory();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            policy =>
            {
                policy.WithOrigins("http://localhost/", "http://localhost","http://localhost:5173/", "http://localhost:5173")
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
    });

    var app = builder.Build();

    app.UseConfigPathBase(configuration);
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.UseRouting();
    app.UseJsonException();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseCors("AllowSpecificOrigin");

    app.MapControllers();
    app.MapHealthChecks("/healthz");
    app.MapHub<CodeHub>("/code");

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex);
    throw;
}
finally
{
    LogManager.Shutdown();
}
