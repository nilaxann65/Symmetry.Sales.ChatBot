using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Embeddings;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.Infrastructure.SemanticServices;

[Description("Representa el inventario")]
#pragma warning disable SKEXP0001
public class InventoryPlugin
#pragma warning restore SKEXP0001
{
  [KernelFunction("get_products")]
  [Description("Get the products availables for sale")]
  public string GetInventory()
  {
    return $"manzanas, peras, sandias";
  }

  [KernelFunction("get_product_by_name")]
  [Description("Get a product by its name, if it doesnt exists, returns null")]
  public string GetProductByName(string name)
  {
    return "el producto es muy delicioso";
  }
}
