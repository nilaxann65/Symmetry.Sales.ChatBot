using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.Logging;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;
using Symmetry.Sales.ChatBot.Core.Utils;

namespace Symmetry.Sales.ChatBot.UseCases.Companies.Payments.AddPaymentMethod;

public class AddPaymentMethodHandler(
  IRepository<Business> businessRepository,
  ILogger<AddPaymentMethodHandler> logger
) : ICommandHandler<AddPaymentMethodCommand, Result>
{
  public async Task<Result> Handle(
    AddPaymentMethodCommand request,
    CancellationToken cancellationToken
  )
  {
    var business = await businessRepository.GetByIdAsync(
      ContextAccesor.CurrentTenantId,
      cancellationToken
    );

    if (business is null)
    {
      logger.LogError("Business not found");
      return Result.NotFound("Business not found");
    }

    business.AddPaymentMethod(request.name, request.type, request.paymentDetails);

    await businessRepository.SaveChangesAsync(cancellationToken);

    return Result.Success();
  }
}
