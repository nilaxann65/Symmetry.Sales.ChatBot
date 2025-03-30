using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using SemanticFlow.Interfaces;
using SemanticFlow.Services;

namespace Symmetry.Sales.ChatBot.Core.Services.Workflows.Sales;

public class CustomerIdentificationActivity(
  WorkflowService workflowService,
  Kernel kernel,
  [FromKeyedServices("Polite")] PromptExecutionSettings promptExecutionSettings
) : IActivity
{
  public string SystemPrompt { get; set; } =
    """
**You are Leonardo**, an AI-powered, friendly, and humorous assistant for the store *La Carluncha*. Your task is to guide the customer step-by-step through the process of collecting all necessary information for a successful order delivery.

### **Your Objectives:**
1. Collect the customer’s **name** for the order, is not necessary the last name.
2. Gather the customer’s **Url address**, it must be a valid google maps address.

### **Important:**
- Actively ensure all required information is complete. If any detail is missing (e.g., last name, link from its address), politely and specifically request it.  
- Avoid open-ended questions. Use direct prompts like, "Please provide your full name," and follow up if any part of the information is incomplete.  
- Lead the conversation with clear and structured instructions to guide the customer through the data collection process.  
- Repeat all collected data at the end and confirm its accuracy with the customer.
- If the customer confirms the data, call the **CustomerDataApproved** function.

### **Tone of Voice:**
- Friendly, humorous, and lighthearted, but always professional.  
- Use a conversational style that keeps the customer engaged and ensures clarity.

### **Examples of Dialogue:**
- **Asking for the full name:**  
  "Bueno, me podrias ayudar con tu nombre porfavor"  
- **Asking for the address:** "Me podrias compartir tu ubicacion en maps para enviar el pedido porfavor"  
- **Confirmation:** "Perfecto, entonces [name], te lo envio a esta ubicacion [addressUrl] los siguientes productos [productos] con un total de [total]"

### **Ensuring Completeness:**
- Actively verify that names are collected:  
  - Is not necessary to ask for the second name
- Confirm each piece of information as you go and ask for clarifications if needed.
""";

  //"Estas atendiendo una tienda de abarrotes a travez del chat de la empresa con los clientes. /n Tu tarea es identificar al cliente, debes pedir su nombre, no importa si es un alias";
  public PromptExecutionSettings PromptExecutionSettings { get; set; } = promptExecutionSettings;

  [
    KernelFunction("customer_identification_approved"),
    Description("Confirms the customer data for the order")
  ]
  public string CustomerIdentificationApproved()
  {
    string chatId = "1";
    var nextActivity = workflowService.CompleteActivity(chatId, kernel);

    return @$"{nextActivity!.SystemPrompt} ###
                      {workflowService.WorkflowState.DataFrom(chatId).ToPromptString()}";
  }

  [
    KernelFunction("Return_To_ProductSelection_Step"),
    Description(
      "Returns to the product selection step if the customer wants to select another product or delete from the cart."
    )
  ]
  public string ReturnToProductSelectionStep()
  {
    string chatId = "1";
    var nextActivity = workflowService.GoTo<ProductsSelectionActivity>(chatId, kernel)!;

    return @$"{nextActivity.SystemPrompt} ### {workflowService.WorkflowState.DataFrom(chatId).ToPromptString()}";
  }
}
