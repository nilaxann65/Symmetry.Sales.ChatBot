using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using SemanticFlow.Interfaces;
using SemanticFlow.Services;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.Core.Services.Workflows.Sales;

public class PaymentSelectionActivity(
  WorkflowService workflowService,
  Kernel kernel,
  [FromKeyedServices("Polite")] PromptExecutionSettings promptExecutionSettings
) : IActivity
{
  public string SystemPrompt { get; set; } =
    """
**You are Leonardo**, an AI-powered, friendly, and humorous assistant for the store *La Carluncha*. Your task is to collect the payment method preferred by the customer.

### **Your Objectives:**
1. Offer to the customer the active payment methods using the tool ***get_payment_methods***.
2. Collect one payment method provided by our list of allowed payment methods.

### **Tone of Voice:**
- Friendly, humorous, and lighthearted, but always professional.  
- Use a conversational style that keeps the customer engaged and ensures clarity.

### **Examples of Dialogue:**  
- **Confirmation:** "Perfecto, te comparto los datos para el pago con paypal,: Cuenta: 77272888373 Banco: Bancosol"
""";
  public PromptExecutionSettings PromptExecutionSettings { get; set; } = promptExecutionSettings;

  [
    KernelFunction("get_payment_methods"),
    Description("Get a list of the active payment methods for the purchase")
  ]
  [return: Description("return an array of names of the availables payment methods")]
  public string GetPaymentMethods()
  {
    return "[Paypal]";
  }

  [
    KernelFunction("get_payment_method_details_by_name"),
    Description(
      "Get the data needed to make the payment by the name of the payment method collected from the method ***get_payment_methods***"
    )
  ]
  [return: Description("Returns a single object of full details from a payment method")]
  public string GetPaymentMethodDetails([Description("payment Method name")] string name)
  {
    return JsonSerializer.Serialize(
      new PaymentMethod(
        "PayPal",
        PaymentMethodType.PayPal,
        "Cuenta: 77272888373 Destinatario: JOSE ARMANDO VARGAS AGUANTA Banco: Bancosol"
      )
    );
  }

  [
    KernelFunction("confirm_payment_method_selected"),
    Description("Saves the payment method selected by the customer")
  ]
  public string ConfirmPaymentMethodSelected(string paymentMethod)
  {
    string chatId = "1";
    var nextActivity = workflowService.CompleteActivity(chatId, kernel);

    return @$"{nextActivity!.SystemPrompt} ###
                      {workflowService.WorkflowState.DataFrom(chatId).ToPromptString()}";
  }
}
