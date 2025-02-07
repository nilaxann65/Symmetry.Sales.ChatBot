using Ardalis.GuardClauses;
using Ardalis.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Symmetry.Sales.ChatBot.Core.Interfaces;
using Symmetry.Sales.ChatBot.Core.Services;
using Symmetry.Sales.ChatBot.Infrastructure.Data;
using Symmetry.Sales.ChatBot.Infrastructure.Data.Queries;
using Symmetry.Sales.ChatBot.Infrastructure.Email;
using Symmetry.Sales.ChatBot.Infrastructure.Services.ChatModels;
using Symmetry.Sales.ChatBot.Infrastructure.Services.ChatModels.Config;
using Symmetry.Sales.ChatBot.UseCases.Contributors.List;

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
    services.AddScoped<IListContributorsQueryService, ListContributorsQueryService>();
    services.AddScoped<IDeleteContributorService, DeleteContributorService>();

    services.Configure<MailserverConfiguration>(config.GetSection("Mailserver"));

    logger.LogInformation("{Project} services registered", "Infrastructure");

    return services;
  }

  public static IServiceCollection AddSemanticKernel(
    this IServiceCollection services,
    IConfiguration configuration
  )
  {
    services.AddKernel();

    services.AddSingleton<IModels, Models>();

    Dictionary<string, SemanticKernelDetailOptions> options = configuration
      .GetSection("SemanticKernelModels")
      .Get<Dictionary<string, SemanticKernelDetailOptions>>()!;

    string geminiApiKey = options.GetValueOrDefault("gemini")?.ApiKey!;
    string geminiModel = "gemini-1.5-flash-8b";

#pragma warning disable SKEXP0070
    services.AddGoogleAIGeminiChatCompletion(geminiModel, geminiApiKey);
#pragma warning restore SKEXP0070
    return services;
  }
}
