using System.Net.Http.Json;
using Ardalis.Result;
using Symmetry.Sales.ChatBot.Core.Interfaces;
using Symmetry.Sales.ChatBot.Infrastructure.Services.MessagingServices.WhatsappService.Requests;

namespace Symmetry.Sales.ChatBot.Infrastructure.Services.MessagingServices.WhatsappService;

internal class WhatsappMessagingService(HttpClient httpClient) : IMessagingService
{
  public async Task<Result> SendTextMessageAsync(
    string fromId,
    string destination,
    string message,
    bool previewUrl,
    CancellationToken cancellationToken
  )
  {
    var messageRequest = new SendTextMessageRequest()
    {
      To = destination,
      Text = new SendTextMessageRequest.TextContent() { Body = message, preview_url = previewUrl }
    };
    var response = await httpClient.PostAsJsonAsync(
      $"/v22.0/{fromId}/messages",
      messageRequest,
      cancellationToken: cancellationToken
    );

    string error = await response.Content.ReadAsStringAsync();
    return response.IsSuccessStatusCode
      ? Result.Success()
      : Result.CriticalError("Failed to send message to whatsapp api");
  }
}
