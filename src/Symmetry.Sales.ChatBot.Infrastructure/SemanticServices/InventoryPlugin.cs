using System.ComponentModel;
using Microsoft.SemanticKernel;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;
using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.Infrastructure.SemanticServices;

[Description("Representa el inventario")]
public class InventoryPlugin(IProductService productService)
{
  [KernelFunction("get_products")]
  [Description("Get the products availables for sale")]
  public string GetInventory()
  {
    return $"manzanas, peras, sandias";
  }

  [KernelFunction("get_product_by_name")]
  [Description(
    "Get a product by its name, if it doesnt exists, you will get a null and have to tell the user we dont have it"
  )]
  public async Task<IProduct?> GetProductByName(string name)
  {
    var result = await productService.GetProductByDescriptionAsync(name);
    if (result.IsSuccess)
      return result.Value;

    return null;
  }
}
