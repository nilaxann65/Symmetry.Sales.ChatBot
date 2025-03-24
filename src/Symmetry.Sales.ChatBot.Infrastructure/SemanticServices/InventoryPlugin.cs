using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;
using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.Infrastructure.SemanticServices;

[Description("Representa el inventario")]
public class InventoryPlugin(IProductService productService, ILogger<InventoryPlugin> logger)
{
  [KernelFunction("get_products")]
  [Description("Get the products availables for sale")]
  public string GetInventory()
  {
    return $"manzanas, peras, sandias";
  }

  [KernelFunction("get_product_by_description")]
  [Description("Get description, price and stock of a product by a description ")]
  public async Task<IProduct?> GetProductByDescription(string description)
  {
    logger.LogInformation("GetProductByName: {description}", description);
    var result = await productService.GetProductByDescriptionAsync(description);
    if (result.IsSuccess)
      return result.Value;

    return null;
  }
}
