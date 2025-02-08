using Symmetry.Sales.ChatBot.Core.ChatAggregate;

namespace Symmetry.Sales.ChatBot.Web.Endpoints.Chats;

public class StartChatRequest
{
  public static string Route = "Chat/Start/Generate";
  public string UserMessage { get; set; } = string.Empty;
  public string ContactId { get; set; } = string.Empty;
  public ChatOrigin ChatOrigin { get; set; }
}
