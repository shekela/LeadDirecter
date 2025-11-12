using Serilog;
using OpenTelemetry.Metrics;
using LeadDirecter.Service.Services.CampaignsService;
using LeadDirecter.Data.Context;
using Microsoft.EntityFrameworkCore;
using LeadDirecter.Data.Repository.CampaignRepository;
using LeadDirecter.Service.Services.DestinationCrmConfigurationService;
using LeadDirecter.Data.Repository.DestinationCrmConfigurationRepository;
using LeadDirecter.WebApi.Middlewares;
using Serilog.Sinks.Elasticsearch;
using LeadDirecter.Data.Repository.LeadRepository;
using LeadDirecter.Service.Services.LeadsService;
using LeadDirecter.Service.Services.RequestBuilderService;
using LeadDirecter.Service.Services.SenderService;
using LeadDirecter.Service.Services.LeadPersistenceService;
using FluentValidation;
using LeadDirecter.Service.RequestValidators;
using LeadDirecter.Data.Repository.CampaignValidationsRepository;
using LeadDirecter.Data.Repository.LeadValidationsRepository;
using LeadDirecter.Service.Validations.LeadValidationsService;
using LeadDirecter.Service.Validations.CampaignValidationsService;


var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// Configure Serilog
// ---------------------------

Serilog.Debugging.SelfLog.Enable(msg =>
{
    Console.WriteLine("Serilog SelfLog: " + msg);
});

var elasticUri = new Uri(builder.Configuration["Elasticsearch:Uri"]);
var username = builder.Configuration["Elasticsearch:Username"];
var password = builder.Configuration["Elasticsearch:Password"];
var logIndexFormat = builder.Configuration["Elasticsearch:LogIndexFormat"];

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("Service", "LeadDirecter")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.Logger(lc => lc
        .Filter.ByExcluding(le =>
            le.Properties.ContainsKey("RequestPath") &&
            le.Properties["RequestPath"].ToString().Trim('"') == "/metrics"
        )
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elasticUri)
        {
            IndexFormat = logIndexFormat,
            AutoRegisterTemplate = true,
            ModifyConnectionSettings = x => x
                .BasicAuthentication(username, password)
                .ServerCertificateValidationCallback((o, cert, chain, errors) => true)
        })
    )
    .CreateLogger();

Log.Information("Serilog with Elasticsearch configured successfully");

builder.Host.UseSerilog();



// ---------------------------
// Add Services
// ---------------------------
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<IDestinationCrmConfigurationService, DestinationCrmConfigurationService>();
builder.Services.AddScoped<ILeadService, LeadService>();
builder.Services.AddScoped<IRequestBuilder, RequestBuilder>();
builder.Services.AddScoped<ILeadSender, LeadSender>();
builder.Services.AddScoped<ILeadPersistenceService, LeadPersistenceService>();
builder.Services.AddScoped<ILeadValidationService, LeadValidationService>();
builder.Services.AddScoped<ICampaignValidationService, CampaignValidationService>();


// ---------------------------
// Add Repositories
// ---------------------------
builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
builder.Services.AddScoped<IDestinationCrmConfigurationRepository, DestinationCrmConfigurationRepository>();
builder.Services.AddScoped<ILeadRepository, LeadRepository>();
builder.Services.AddScoped<ICampaignValidationRepository, CampaignValidationRepository>();
builder.Services.AddScoped<ILeadValidationRepository, LeadValidationRepository>();

// ---------------------------
// Add MVC and Swagger
// ---------------------------
builder.Services.AddControllers();

// Register all validators from your assembly
builder.Services.AddValidatorsFromAssemblyContaining<CampaignRequestValidator>();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("LeadDirecterDb")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddPrometheusExporter();
    });


var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint(); 

// ---------------------------
// Middleware pipeline
// ---------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCorrelationId();
app.UseMiddleware<ValidationMiddleware>();
app.MapControllers();
app.Run();
