using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.Web.Endpoints.Chats;

public class StartChatRequest
{
  public static string Route = "Chat/Start/Generate";
  public string UserMessage { get; set; } = string.Empty;
  public string ContactId { get; set; } = string.Empty;
  public Channel ChatOrigin { get; set; }
  public int TenantId { get; set; }
}
