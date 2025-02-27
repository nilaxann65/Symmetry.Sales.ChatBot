using Ardalis.Result;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel.Embeddings;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;
using Symmetry.Sales.ChatBot.Core.Interfaces;
using Symmetry.Sales.ChatBot.Infrastructure.SemanticServices.Entities;

namespace Symmetry.Sales.ChatBot.Infrastructure.SemanticServices.Services;

#pragma warning disable SKEXP0001
public class QdrantProductService(
  ITextEmbeddingGenerationService textEmbeddingGenerationService,
  IVectorStoreRecordCollection<Guid, ProductEntity> productCollection,
  ILogger<QdrantProductService> logger
) : IProductService
#pragma warning restore SKEXP0001
{
  public async Task<Result> AddProductAsync(
    Guid id,
    string name,
    string description,
    double price,
    string[] tags,
    CancellationToken ct = default
  )
  {
    //Create collection if it doesn't exist
    await productCollection.CreateCollectionIfNotExistsAsync(ct);

    var textEmbedding = await textEmbeddingGenerationService.GenerateEmbeddingAsync(
      description,
      cancellationToken: ct
    );
    logger.LogInformation("Embedding generated");

    var newProduct = new ProductEntity
    {
      Id = id,
      Name = name,
      BusinessId = 1, //TODO cambiar
      Description = description,
      Price = price,
      Tags = tags,
      Embedding = textEmbedding.ToArray(),
    };

    var upsertResult = await productCollection.UpsertAsync(newProduct, cancellationToken: ct);
    if (upsertResult == Guid.Empty)
    {
      logger.LogError("Error upserting product {id}", id);
      return Result.Error("Error upserting the product");
    }

    logger.LogInformation("Product {id} upserted", id);

    return Result.Success();
  }

  public Task<Result> DeleteProductByIdAsync(Guid id, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }

  public async Task<Result<IProduct>> GetProductByDescriptionAsync(
    string description,
    CancellationToken ct = default
  )
  {
    var textEmbedding = await textEmbeddingGenerationService.GenerateEmbeddingAsync(
      description,
      cancellationToken: ct
    );

    logger.LogInformation("Embedding generated");

    var vectorSearchOptions = new VectorSearchOptions
    {
      Filter = new VectorSearchFilter().EqualTo(nameof(ProductEntity.BusinessId), 1), //TODO hacer dinamico el businessId
      Top = 5,
    };

    var results = await productCollection.VectorizedSearchAsync(
      textEmbedding,
      vectorSearchOptions,
      cancellationToken: ct
    );

    if (results.TotalCount == 0)
    {
      logger.LogInformation("No products found for description {description}", description);
      return Result<IProduct>.NotFound();
    }

    var products = results.Results.ToBlockingEnumerable(cancellationToken: ct);

    foreach (var product in products)
    {
      logger.LogInformation(
        "Product found: {product} with score {score}",
        product.Record.Name,
        product.Score
      );
    }

    var result = products.First();

    return result.Record;
  }

  public Task<Result<IProduct>> GetProductByIdAsync(Guid id, CancellationToken ct = default)
  {
    throw new NotImplementedException();
  }
}
