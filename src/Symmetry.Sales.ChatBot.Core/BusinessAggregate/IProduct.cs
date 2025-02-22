namespace Symmetry.Sales.ChatBot.Core.BusinessAggregate;

public interface IProduct
{
  public Guid Id { get; set; }
  public int BusinessId { get; set; }
  public string Name { get; set; }
  public string Description { get; set; }
  public double Price { get; set; }
  public ReadOnlyMemory<float>? Embedding { get; set; }
  public string[] Tags { get; set; }
}
