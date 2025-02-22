using System;

namespace Symmetry.Sales.ChatBot.Web.Endpoints.Companies.Products;

public class CreateRequest
{
  public static string Route = "Business/Products/Execute";

  public string Name { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public double Price { get; set; }
  public string[] Tags { get; set; } = [];
}
