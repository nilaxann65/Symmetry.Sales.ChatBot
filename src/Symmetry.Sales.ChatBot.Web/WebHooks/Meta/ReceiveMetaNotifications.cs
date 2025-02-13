using FastEndpoints;
using MediatR;
using Symmetry.Sales.ChatBot.UseCases.Chats.AnswerMessage;

namespace Symmetry.Sales.ChatBot.Web.WebHooks.Meta;

public class ReceiveMetaNotifications(IMediator mediator)
  : Endpoint<ReceiveMetaNotificationsRequest>
{
  public override void Configure()
  {
    Post(ReceiveMetaNotificationsRequest.Route);
    AllowAnonymous();
    Summary(s => s.Summary = "Receive meta notifications from the chatbot");
  }

  public override async Task HandleAsync(ReceiveMetaNotificationsRequest req, CancellationToken ct)
  {
    if (req.Object == "whatsapp_business_account") //Notificaciones whatsapp
    {
      foreach (var entry in req.Entry)
      {
        foreach (var change in entry.Changes)
        {
          if (change.Field == "messages") //Messages
          {
            if (change.Value.Messaging_Product == "whatsapp")
            {
              var command = new AnswerMessageCommand(
                change.Value.Contacts.First().Wa_Id,
                change.Value.Metadata.Phone_Number_Id,
                Core.BusinessAggregate.Channel.Whatsapp,
                change.Value.Messages.First().Text.Body
              );

              await mediator.Send(command, ct);
            }
          }
        }
      }
    }
    await SendOkAsync(ct);
  }
}
