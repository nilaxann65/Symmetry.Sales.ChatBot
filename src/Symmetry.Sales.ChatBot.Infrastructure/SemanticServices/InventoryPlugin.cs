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

  [KernelFunction("get_product_by_name")]
  [Description("Get description, price and stock of a product by a similar name")]
  public async Task<IProduct?> GetProductByName(string name)
  {
    logger.LogInformation("GetProductByName: {name}", name);
    var result = await productService.GetProductByDescriptionAsync(name);
    if (result.IsSuccess)
      return result.Value;

    return null;
  }
}
