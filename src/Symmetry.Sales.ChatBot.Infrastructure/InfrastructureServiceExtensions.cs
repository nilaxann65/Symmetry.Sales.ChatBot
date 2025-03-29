using System.Net.Http.Headers;
using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Qdrant;
using Qdrant.Client;
using Symmetry.Sales.ChatBot.Core.Interfaces;
using Symmetry.Sales.ChatBot.Core.Services;
using Symmetry.Sales.ChatBot.Infrastructure.Data;
using Symmetry.Sales.ChatBot.Infrastructure.Email;
using Symmetry.Sales.ChatBot.Infrastructure.SemanticServices.Entities;
using Symmetry.Sales.ChatBot.Infrastructure.SemanticServices.Services;
using Symmetry.Sales.ChatBot.Infrastructure.Services.MessagingServices.WhatsappService;
using Symmetry.Sales.ChatBot.Infrastructure.Services.Meta.Config;
using Symmetry.Sales.ChatBot.Infrastructure.Services.SemanticKernel;
using Symmetry.Sales.ChatBot.Infrastructure.Services.SemanticKernel.Config;

namespace Symmetry.Sales.ChatBot.Infrastructure;

public static class InfrastructureServiceExtensions
{
  public static IServiceCollection AddInfrastructureServices(
    this IServiceCollection services,
    ConfigurationManager config
  )
  {
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    string? connectionString = config.GetConnectionString("DefaultConnection");
    Guard.Against.Null(connectionString);

    services.AddSemanticKernel(config);

    services.AddDbContext<AppDbContext>(options =>
      options.UseNpgsql(connectionString, s => s.MigrationsAssembly("Symmetry.Sales.ChatBot.Web"))
    );

    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
    services.AddScoped(typeof(IReadRepository<>), typeof(EfRepository<>));

    services.Configure<MailserverConfiguration>(config.GetSection("Mailserver"));

    services.AddMetaServices(config);

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
    Dictionary<string, SemanticKernelDetailOptions> options = configuration
      .GetSection("SemanticKernelModels")
      .Get<Dictionary<string, SemanticKernelDetailOptions>>()!;

    string geminiApiKey = options.GetValueOrDefault("gemini")?.ApiKey!;

    string deepseekApiKey = options.GetValueOrDefault("deepseek")?.ApiKey!;

    services.AddSingleton<IModels, Models>();

#pragma warning disable SKEXP0070

    services
      .AddKernel()
      .AddGoogleAIGeminiChatCompletion("gemini-2.0-flash", geminiApiKey)
      .AddGoogleAIGeminiChatCompletion("gemini-2.0-flash-lite", geminiApiKey)
      .AddGoogleAIEmbeddingGeneration("text-embedding-004", geminiApiKey);

    services.AddConversationFlow();

    services.AddModels();
#pragma warning restore SKEXP0070

    #region Vector Stores
    services.AddTransient<IProductService, QdrantProductService>();

    var vectorStore = new QdrantVectorStore(new QdrantClient("192.168.0.18"));
    services.AddTransient(sp => vectorStore);
    var productCollection = vectorStore.GetCollection<Guid, ProductEntity>("SKProducts");

    services.AddTransient(sp => productCollection);
    #endregion

    services.AddTransient<IMessageProcessingService, SemanticKernelService>();
    return services;
  }
}
