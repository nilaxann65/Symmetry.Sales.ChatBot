using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;
using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.UseCases.Companies.Products.Create;

public class CreateProductHandler(
  IProductService productService,
  ILogger<CreateProductHandler> logger
) : ICommandHandler<CreateProductCommand, Result<Guid>>
{
  public async Task<Result<Guid>> Handle(
    CreateProductCommand request,
    CancellationToken cancellationToken
  )
  {
    var newGuid = Guid.NewGuid();

    logger.LogInformation("Creating product with id {newGuid}", newGuid);
    var newProductResult = await productService.AddProductAsync(
      newGuid,
      request.name,
      request.description,
      request.price,
      request.tags,
      cancellationToken
    );

    return newProductResult.Map(result => newGuid);
  }
}
