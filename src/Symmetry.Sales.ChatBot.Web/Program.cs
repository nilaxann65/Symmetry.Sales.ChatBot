using System.Reflection;
using System.Text;
using Ardalis.ListStartupServices;
using Ardalis.SharedKernel;
using Destructurama;
using FastEndpoints;
using FastEndpoints.Swagger;
using MediatR;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;
using Symmetry.Sales.ChatBot.Core.Interfaces;
using Symmetry.Sales.ChatBot.Infrastructure;
using Symmetry.Sales.ChatBot.Infrastructure.Data;
using Symmetry.Sales.ChatBot.Infrastructure.Email;
using Symmetry.Sales.ChatBot.UseCases.Chats.StartChat;
using Symmetry.Sales.ChatBot.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

var connectionStringAzureApp = builder.Configuration.GetValue<string>(
  "AzureAppConfiguration:AppConfig"
);

// Configure Web Behavior
builder.Services.Configure<CookiePolicyOptions>(options =>
{
  options.CheckConsentNeeded = context => true;
  options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddCors(options =>
{
  options.AddPolicy(
    "AllowAll",
    policy =>
    {
      policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    }
  );
});
builder
  .Services.AddFastEndpoints()
  .SwaggerDocument(o =>
  {
    o.ShortSchemaNames = true;
  });

builder.Services.AddApplicationInsightsTelemetry();

builder.Services.Configure<TelemetryConfiguration>(options =>
  options.TelemetryInitializers.Add(new AppInsightsTelemetryInitializer())
);

ConfigureMediatR();

builder.Services.AddInfrastructureServices(builder.Configuration);

if (builder.Environment.IsDevelopment())
{
  // Use a local test email server
  // See: https://ardalis.com/configuring-a-local-test-email-server/
  builder.Services.AddScoped<IEmailSender, MimeKitEmailSender>();

  // Otherwise use this:
  //builder.Services.AddScoped<IEmailSender, FakeEmailSender>();
  AddShowAllServicesSupport();
}
else
{
  builder.Services.AddScoped<IEmailSender, MimeKitEmailSender>();
}

var app = builder.Build();

Log.Logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .WriteTo.ApplicationInsights(
    app.Services.GetRequiredService<TelemetryConfiguration>(),
    TelemetryConverter.Traces
  )
  .Destructure.ByTransforming<IMediator>(mediator => new { }) // Evita loguear los campos de IMediator
  .Destructure.UsingAttributes()
  .CreateLogger();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseShowAllServicesMiddleware(); // see https://github.com/ardalis/AspNetCoreStartupServices
}
else
{
  app.UseDefaultExceptionHandler(); // from FastEndpoints
  app.UseHsts();
}

app.UseCors("AllowAll");

app.UseFastEndpoints().UseSwaggerGen(); // Includes AddFileServer and static files middleware

app.UseHttpsRedirection();

SeedDatabase(app);

app.Run();

static void SeedDatabase(WebApplication app)
{
  using var scope = app.Services.CreateScope();
  var services = scope.ServiceProvider;

  try
  {
    var context = services.GetRequiredService<AppDbContext>();
    //          context.Database.Migrate();
    context.Database.EnsureCreated();
    SeedData.Initialize(services);
  }
  catch (Exception ex)
  {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
  }
}

void ConfigureMediatR()
{
  var mediatRAssemblies = new[]
  {
    Assembly.GetAssembly(typeof(Chat)), // Core
    Assembly.GetAssembly(typeof(StartChatCommand)), // UseCases
  };
  builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(mediatRAssemblies!));
  builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
  builder.Services.AddScoped<IDomainEventDispatcher, MediatRDomainEventDispatcher>();
}

void AddShowAllServicesSupport()
{
  // add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
  builder.Services.Configure<ServiceConfig>(config =>
  {
    config.Services = new List<ServiceDescriptor>(builder.Services);

    // optional - default path to view services is /listallservices - recommended to choose your own path
    config.Path = "/listservices";
  });
}

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program { }
