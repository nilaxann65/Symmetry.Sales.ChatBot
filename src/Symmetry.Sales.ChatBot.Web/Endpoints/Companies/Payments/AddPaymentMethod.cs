using FastEndpoints;
using MediatR;
using Symmetry.Sales.ChatBot.Core.Utils;
using Symmetry.Sales.ChatBot.UseCases.Companies.Payments.AddPaymentMethod;

namespace Symmetry.Sales.ChatBot.Web.Endpoints.Companies.Payments;

public class AddPaymentMethod(IMediator mediator) : Endpoint<AddPaymentMethodRequest>
{
  public override void Configure()
  {
    Post(AddPaymentMethodRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Add a payment method to a business";
    });
  }

  public override async Task HandleAsync(AddPaymentMethodRequest req, CancellationToken ct)
  {
    ContextAccesor.CurrentTenantId = 1;
    var command = new AddPaymentMethodCommand(req.Name, req.Type, req.PaymentDetails);
    var response = await mediator.Send(command, ct);

    if (response.IsSuccess)
    {
      await SendOkAsync(ct);
    }
    else
    {
      await SendForbiddenAsync(ct);
    }
  }
}
