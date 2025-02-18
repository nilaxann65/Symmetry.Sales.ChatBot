using System.Text;
using FastEndpoints;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Symmetry.Sales.ChatBot.UseCases.Chats.AnswerMessage;

namespace Symmetry.Sales.ChatBot.Web.WebHooks.Meta;

public class ReceiveMetaNotifications(IMediator mediator, ILogger<ReceiveMetaNotifications> logger)
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
    logger.LogInformation("Meta notification received, {@object}", req);

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
              if (change.Value.Contacts.Any() && change.Value.Messages.Any())
              {
                ChangeTelemetryName("Whatsapp Message");

                logger.LogInformation(
                  "Message received from {Wa_Id}",
                  change.Value.Contacts.First().Wa_Id
                );

                var command = new AnswerMessageCommand(
                  change.Value.Contacts.First().Wa_Id,
                  change.Value.Metadata.Phone_Number_Id,
                  Core.BusinessAggregate.Channel.Whatsapp,
                  change.Value.Messages.First().Text.Body
                );

                try
                {
                  await mediator.Send(command, ct);
                  await SendOkAsync(ct);
                }
                catch
                {
                  ChangeTelemetryName("Whatsapp Message with Error");
                  await SendOkAsync(ct);
                }
              }
              else
              {
                ChangeTelemetryName("Whatsapp Message status update");
                logger.LogInformation("No contacts or messages found in the notification");
                await SendOkAsync(ct);
              }
            }
          }
        }
      }
    }
  }

  public void ChangeTelemetryName(string name)
  {
    var requestTelemetry = HttpContext.Features.Get<RequestTelemetry>();
    if (requestTelemetry != null)
    {
      requestTelemetry.Name = name;
    }
  }
}
