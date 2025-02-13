using FastEndpoints;
using MediatR;
using Symmetry.Sales.ChatBot.UseCases.Messages.Generate;

namespace Symmetry.Sales.ChatBot.Web.Endpoints.Chats;

public class GenerateMessage(IMediator mediator)
  : Endpoint<GenerateMessageRequest, GenerateMessageResponse>
{
  public override void Configure()
  {
    Post(GenerateMessageRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Generate a message";
      s.Description = "Generate a message for a chat";
    });
  }

  public override async Task HandleAsync(GenerateMessageRequest req, CancellationToken ct)
  {
    var command = new GenerateMessageCommand(
      req.UserMessage,
      req.ContactId,
      req.ChatOrigin,
      req.TenantId
    );

    var response = await mediator.Send(command, ct);

    if (response.IsSuccess)
      Response = new() { Content = response.Value };
    else
      await SendNotFoundAsync(ct);
  }
}
