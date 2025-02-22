using Microsoft.Extensions.VectorData;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.Infrastructure.SemanticServices.Entities;

public class ProductEntity : IProduct
{
  [VectorStoreRecordKey]
  public Guid Id { get; set; }

  [VectorStoreRecordData]
  public int BusinessId { get; set; }

  [VectorStoreRecordData(IsFilterable = true)]
  public string Name { get; set; } = string.Empty;

  [VectorStoreRecordData(IsFullTextSearchable = true)]
  public string Description { get; set; } = string.Empty;

  [VectorStoreRecordData(IsFilterable = true)]
  public double Price { get; set; }

  [VectorStoreRecordVector(Dimensions: 768, DistanceFunction.CosineSimilarity, IndexKind.Hnsw)]
  public ReadOnlyMemory<float>? Embedding { get; set; }

  [VectorStoreRecordData(IsFilterable = true)]
  public string[] Tags { get; set; } = [];
}
