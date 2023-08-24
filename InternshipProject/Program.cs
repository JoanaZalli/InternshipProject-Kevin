using Application;
using Hangfire;
using Infrastructure;
using Infrastructure.Persistence.Context;
using Infrastructure.Services.Authentication.PermissionPolicyConfigurations;
using Infrastructure.Services.Hangfire.DashboardAuthorization;
using InternshipProject.Middleware;
using InternshipProject.SwaggerConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddApplicationLayer(builder.Configuration)
    .AddInfrastructureLayer(builder.Configuration);

builder.Services.AddLocalization();

var supportedCultures = new[] {
        new CultureInfo("en-US"),
        new CultureInfo("it-IT"),
    };
var localizationOptions = new RequestLocalizationOptions {
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    DefaultRequestCulture = new RequestCulture("en-US"),
    ApplyCurrentCultureToResponseHeaders = true
};

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient();

builder.Services.AddAuthorization();

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opt => {
    opt.EnableAnnotations();
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.OperationFilter<AcceptLanguageOperationFilter>();
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement { {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

ConfigureLogging();
builder.Host.UseSerilog();

var app = builder.Build();

app.UseRequestLocalization(localizationOptions);

// automatically apply migrations
using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions() {
    Authorization = new[] {
        new HangfireAuthorizationFilter()
    }
});

app.MapControllers();

app.Run();

static void ConfigureLogging() {
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environment}.json", optional: true)
        .Build();

#pragma warning disable CS8604 // Possible null reference argument.
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .Enrich.WithMachineName()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(config["ElasticConfiguration:Uri"])) {
            AutoRegisterTemplate = true,
            IndexFormat = $"{Assembly.GetExecutingAssembly()?.GetName()?.Name?.ToLower().Replace(".", "-")}-{environment?.ToLower()}-{DateTime.UtcNow:yyyy-MM-dd}",
            NumberOfReplicas = 1,
            NumberOfShards = 2
        })
        .Enrich.WithProperty("Environment", environment)
        .ReadFrom.Configuration(config)
        .CreateLogger();
#pragma warning restore CS8604 // Possible null reference argument.

}

public partial class Program { }