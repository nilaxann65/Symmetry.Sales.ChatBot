using System;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;
using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.UseCases.Companies.Products.GetByDescription;

public class GetByDescriptionHandler(
  IProductService productService,
  ILogger<GetByDescriptionHandler> logger
) : ICommandHandler<GetByDescriptionCommand, Result<string>>
{
  public async Task<Result<string>> Handle(
    GetByDescriptionCommand request,
    CancellationToken cancellationToken
  )
  {
    logger.LogInformation("Getting product by description: {description}", request.description);
    var result = await productService.GetProductByDescriptionAsync(
      request.description,
      cancellationToken
    );

    return result.Map(product => product.Name);
  }
}
