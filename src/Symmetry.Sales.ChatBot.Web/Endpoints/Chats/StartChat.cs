using FastEndpoints;
using MediatR;
using Symmetry.Sales.ChatBot.UseCases.Chats.StartChat;

namespace Symmetry.Sales.ChatBot.Web.Endpoints.Chats;

public class StartChat(IMediator mediator) : Endpoint<StartChatRequest, StartChatResponse>
{
  public override void Configure()
  {
    Post(StartChatRequest.Route);
    AllowAnonymous();
    Summary(s =>
    {
      s.Summary = "Initiate a chat with the bot";
      s.Description = "Start a chat with the bot by sending a message";
    });
  }

  public override async Task HandleAsync(StartChatRequest req, CancellationToken ct)
  {
    var command = new StartChatCommand(req.UserMessage, req.ContactId, req.ChatOrigin);

    var response = await mediator.Send(command, ct);

    if (response.IsSuccess)
      Response = new StartChatResponse { MessageGenerated = response.Value };
    else
      await SendNotFoundAsync(ct);
  }
}
