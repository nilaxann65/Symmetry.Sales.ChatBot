using Microsoft.Extensions.DependencyInjection;
using SemanticFlow.Extensions;
using Symmetry.Sales.ChatBot.Core.Services.Workflows.Sales;

namespace Symmetry.Sales.ChatBot.Core.Services;

public static class ConversationFlowRegistryExtension
{
  public static IServiceCollection AddConversationFlow(this IServiceCollection services)
  {
    services.AddSalesFlow();
    return services;
  }

  private static void AddSalesFlow(this IServiceCollection services) // problema, no se pueden configurar multiples workflows para el mismo kernel, se debe crear un kernel por workflow (investigar)
  {
    services
      .AddKernelWorkflow()
      .StartWith<ProductsSelectionActivity>()
      .Then<CustomerIdentificationActivity>()
      .Then<PaymentSelectionActivity>()
      .EndsWith<OrderConfirmationActivity>();
  }
}
