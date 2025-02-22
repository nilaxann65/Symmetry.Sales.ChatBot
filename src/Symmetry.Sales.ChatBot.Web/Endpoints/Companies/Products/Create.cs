using System;
using FastEndpoints;
using MediatR;
using Symmetry.Sales.ChatBot.UseCases.Companies.Products.Create;

namespace Symmetry.Sales.ChatBot.Web.Endpoints.Companies.Products;

public class Create(IMediator mediator) : Endpoint<CreateRequest, CreateResponse>
{
  public override void Configure()
  {
    Post(CreateRequest.Route);
    AllowAnonymous();
    Summary(s => s.Summary = "Create a new product");
  }

  public override async Task HandleAsync(CreateRequest req, CancellationToken ct)
  {
    var command = new CreateProductCommand(req.Name, req.Description, req.Price, req.Tags);
    var result = await mediator.Send(command, ct);

    if (result.IsSuccess)
      Response = new CreateResponse { Id = result.Value };
    else
      await SendForbiddenAsync(ct);
  }
}
