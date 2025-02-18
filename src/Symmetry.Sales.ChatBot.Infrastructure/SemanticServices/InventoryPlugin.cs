using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace Symmetry.Sales.ChatBot.Infrastructure.SemanticServices;

[Description("Representa el inventario")]
public class InventoryPlugin
{
  [KernelFunction("get_products")]
  [Description("Get the products availables for sale")]
  public string GetInventory()
  {
    return $"manzanas, peras, sandias";
  }

  [KernelFunction("get_product_description")]
  [Description("Get a product description by its name")]
  public string GetProductDescription(string name)
  {
    return $"La {name} es un producto muy delicioso color rojo, nada antes visto";
  }
}
