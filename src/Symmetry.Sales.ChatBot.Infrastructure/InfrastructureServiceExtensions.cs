using System.Net.Http.Headers;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using Symmetry.Sales.ChatBot.Core.Interfaces;
using Symmetry.Sales.ChatBot.Infrastructure.Data;
using Symmetry.Sales.ChatBot.Infrastructure.Email;
using Symmetry.Sales.ChatBot.Infrastructure.Services.MessagingServices.WhatsappService;
using Symmetry.Sales.ChatBot.Infrastructure.Services.Meta.Config;
using Symmetry.Sales.ChatBot.Infrastructure.Services.SemanticKernel;
using Symmetry.Sales.ChatBot.Infrastructure.Services.SemanticKernel.Config;

namespace Symmetry.Sales.ChatBot.Infrastructure;

public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config,
    ILogger logger
  )
  {
    string? connectionString = config.GetConnectionString("SqliteConnection");
    Guard.Against.Null(connectionString);

    services.AddSemanticKernel(config);

    services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

    services.Configure<MailserverConfiguration>(config.GetSection("Mailserver"));

    services.AddMetaServices(config);

    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }

  public static IServiceCollection AddMetaServices(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    var metaOptions = configuration.GetSection("MetaOptions").Get<MetaOptions>()!;

    services.AddHttpClient<IMessagingService, WhatsappMessagingService>(s =>
    {
      s.BaseAddress = new Uri(metaOptions.Url);
      s.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
        "Bearer",
        metaOptions.Token
      );
    });

    return services;
  }

  public static IServiceCollection AddSemanticKernel(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    services.AddKernel();

    services.AddSingleton<IModels, Models>();

    var models = services.BuildServiceProvider().GetRequiredService<IModels>();
    Dictionary<string, SemanticKernelDetailOptions> options = configuration
      .GetSection("SemanticKernelModels")
      .Get<Dictionary<string, SemanticKernelDetailOptions>>()!;

    string geminiApiKey = options.GetValueOrDefault("gemini")?.ApiKey!;

#pragma warning disable SKEXP0070
    services.AddGoogleAIGeminiChatCompletion(models.Chat, geminiApiKey);
    var config = new GeminiSafetySetting(
      GeminiSafetyCategory.Dangerous,
      GeminiSafetyThreshold.BlockMediumAndAbove
    );

#pragma warning restore SKEXP0070

    services.AddScoped<IMessageProcessingService, SemanticKernelService>();
    return services;
  }
}
