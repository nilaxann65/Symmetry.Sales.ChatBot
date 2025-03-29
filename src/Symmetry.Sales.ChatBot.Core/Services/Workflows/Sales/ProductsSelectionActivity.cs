using System.ComponentModel;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using SemanticFlow.Interfaces;
using SemanticFlow.Services;
using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.Core.Services.Workflows.Sales;

public class ProductsSelectionActivity(
  WorkflowService workflowService,
  Kernel kernel,
  IProductService productService,
  [FromKeyedServices("Polite")] PromptExecutionSettings promptExecutionSettings
) : IActivity
{
  public string SystemPrompt { get; set; } =
    " Estas atendiendo una tienda de abarrotes a travez del chat de la empresa con los clientes. /n Tu tarea es ayudar al cliente a seleccionar los productos que desea comprar, debes pedirle el nombre del producto y la cantidad que desea comprar, debes indicarle al cliente en todo momento el precio de los productos y brindarle una pequena descripcion de los mismos.";
  public PromptExecutionSettings PromptExecutionSettings { get; set; } = promptExecutionSettings;

  [KernelFunction("find_product_by_description")]
  [Description("Find the nearest product by description")]
  [return: Description("A JSON string with the product basic data")]
  public async Task<string> GetProductByDescription(
    [Description("The product description (dont need to be specific, also allows product names)")]
      string description
  )
  {
    var result = await productService.GetProductByDescriptionAsync(description);
    if (result.IsSuccess)
      return JsonSerializer.Serialize(result.Value);

    return "Any product matched the description.";
  }

  [
    KernelFunction("Confirm_ProductSelection"),
    Description("Finalizes the customer's order by confirming their complete product selection.")
  ]
  [return: Description(
    "A confirmation message indicating the products names, quantities and total price."
  )]
  public string ConfirmProductSelection(
    [Description("The full list of products the customer ordered")] string productSelection
  )
  {
    string chatId = "1";

    var nextActivity = workflowService.CompleteActivity(chatId, productSelection, kernel)!;

    return @$"### {nextActivity.SystemPrompt} ### {workflowService.WorkflowState.DataFrom(chatId).ToPromptString()}";
  }
}
