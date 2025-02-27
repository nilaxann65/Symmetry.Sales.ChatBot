using FastEndpoints;
using MediatR;
using Symmetry.Sales.ChatBot.UseCases.Companies.Products.GetByDescription;

namespace Symmetry.Sales.ChatBot.Web.Endpoints.Companies.Products;

public class GetByDescription(IMediator mediator) : Endpoint<GetByDescriptionRequest, string>
{
  public override void Configure()
  {
    Get(GetByDescriptionRequest.Route);
    AllowAnonymous();
    Summary(s => s.Summary = "Get product by description");
  }

  public override async Task HandleAsync(
    GetByDescriptionRequest request,
    CancellationToken cancellationToken
  )
  {
    var command = new GetByDescriptionCommand(request.Description);

    var result = await mediator.Send(command, cancellationToken);

    if (result.IsSuccess)
    {
      Response = result.Value;
    }
    else
    {
      await SendForbiddenAsync(cancellationToken);
    }

    return;
  }
}
