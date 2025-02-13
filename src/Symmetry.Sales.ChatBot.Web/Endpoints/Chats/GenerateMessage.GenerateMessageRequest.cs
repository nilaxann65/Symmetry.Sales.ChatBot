using Symmetry.Sales.ChatBot.Core.BusinessAggregate;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;

namespace Symmetry.Sales.ChatBot.Web.Endpoints.Chats;

public class GenerateMessageRequest
{
  public static string Route = "/chats/generate-message";
  public string UserMessage { get; set; } = string.Empty;
  public string ContactId { get; set; } = string.Empty;
  public Channel ChatOrigin { get; set; }
  public int TenantId { get; set; }
}
