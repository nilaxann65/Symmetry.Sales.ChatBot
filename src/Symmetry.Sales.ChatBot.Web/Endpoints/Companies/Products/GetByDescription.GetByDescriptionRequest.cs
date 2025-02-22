using System;
using FastEndpoints;

namespace Symmetry.Sales.ChatBot.Web.Endpoints.Companies.Products;

public class GetByDescriptionRequest
{
  public static string Route = "/companies/products/get-by-description";

  [FromBody]
  public string Description { get; set; } = string.Empty;
}
