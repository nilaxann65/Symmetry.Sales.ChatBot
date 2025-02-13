namespace Symmetry.Sales.ChatBot.Infrastructure.Services.MessagingServices.WhatsappService.Requests;

internal class SendTextMessageRequest
{
  public string messaging_product { get; set; } = "whatsapp";
  public string recipient_type { get; set; } = "individual";
  public string To { get; set; } = string.Empty;
  public string Type { get; set; } = "text";
  public TextContent Text { get; set; } = new();

  public class TextContent
  {
    public bool preview_url { get; set; }
    public string Body { get; set; } = string.Empty;
  }
}
