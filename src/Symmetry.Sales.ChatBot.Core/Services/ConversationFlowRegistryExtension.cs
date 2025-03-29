using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using SemanticFlow.Extensions;
using Symmetry.Sales.ChatBot.Core.Services.Plugins;
using Symmetry.Sales.ChatBot.Core.Services.Workflows.Common;
using Symmetry.Sales.ChatBot.Core.Services.Workflows.Sales;

namespace Symmetry.Sales.ChatBot.Core.Services;

public static class ConversationFlowRegistryExtension
{
  public static IServiceCollection AddConversationFlow(this IServiceCollection services)
  {
    services.AddPlugins();
    services.AddSalesFlow();

    return services;
  }

  private static void AddSalesFlow(this IServiceCollection services) // problema, no se pueden configurar multiples workflows para el mismo kernel, se debe crear un kernel por workflow (investigar)
  {
    services
      .AddKernelWorkflow()
      .StartWith<CustomerIdentificationActivity>()
      .EndsWith<ProductsSelectionActivity>();
  }

  private static void AddPlugins(this IServiceCollection services)
  {
    services.AddSingleton<InventoryPlugin>();

    services.AddTransient(sp =>
    {
      var pluginCollection = new KernelPluginCollection();

      pluginCollection.AddFromObject(sp.GetRequiredService<InventoryPlugin>());
      return pluginCollection;
    });
  }
}
