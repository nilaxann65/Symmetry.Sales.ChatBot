using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using SemanticFlow.Interfaces;
using SemanticFlow.Services;

namespace Symmetry.Sales.ChatBot.Core.Services.Workflows.Sales;

public class OrderConfirmationActivity(
  WorkflowService workflowService,
  Kernel kernel,
  [FromKeyedServices("Polite")] PromptExecutionSettings promptExecutionSettings
) : IActivity
{
  public string SystemPrompt { get; set; } =
    """
**You are Leonardo**, an AI-powered, friendly, and humorous assistant for the store **La Carluncha**. Your task is to guide the customer step-by-step through the process of collecting all necessary information for a successful order delivery.

### **Your Objectives:**
1. Ask the customer to confirm the order data
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
  public PromptExecutionSettings PromptExecutionSettings { get; set; } = promptExecutionSettings;

  [
    KernelFunction("get_full_resume"),
    Description(
      "get the order resume, tells the user the products selected, the shipping address and the payment method selected with its details"
    )
  ]
  [return: Description("returns a resume from all data collected")]
  public string GetResume()
  {
    return """
{
  "products": [
    {
      "name": "Cerveza",
      "quantity": 2,  
      "price": 2.5
    },
    {
      "name": "Galletas",
      "quantity": 1,
      "price": 1.5
    }
  ],
  "total": 6.5,
  "addressUrl": "https://goo.gl/maps/xyz123",
  "paymentMethod": {
    "name": "PayPal",
    "type": "PayPal",
    "details": "Cuenta: 77272888373 Destinatario: JOSE ARMANDO VARGAS AGUANTA Banco: Bancosol"
  }
}
""";
  }

  [KernelFunction("confirm_order_data"), Description("Confirms the order data with the customer")]
  [return: Description("returns a confirmation message")]
  public string ConfirmOrderData(
    [Description("Full name of the customer")] string name,
    [Description("Url address of the customer")] string addressUrl,
    [Description("Total amount of the order")] double total
  )
  {
    return @$"Perfecto, entonces {name}, te lo envio a esta ubicacion {addressUrl} con un total de {total}";
  }

  [
    KernelFunction("CustomerDataApproved"),
    Description("Call this function when the customer approves the data")
  ]
  [return: Description("returns a confirmation message")]
  public string CustomerDataApproved()
  {
    string chatId = "1";
    var nextActivity = workflowService.CompleteActivity(chatId, kernel);

    return @$"{nextActivity!.SystemPrompt} ###
                      {workflowService.WorkflowState.DataFrom(chatId).ToPromptString()}";
  }
}
