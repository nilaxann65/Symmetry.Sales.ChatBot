namespace Symmetry.Sales.ChatBot.Web.Endpoints.Companies.Products;

public class GetByDescriptionRequest
{
  public static string Route = "/companies/products/get-by-description";
  public string Description { get; set; } = string.Empty;
}
