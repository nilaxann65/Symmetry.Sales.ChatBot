using FastEndpoints;

namespace Symmetry.Sales.ChatBot.Web.WebHooks.Meta;

public class VerifyRequest
{
  public static string Route = "/webhook";

  [BindFrom("hub.mode")]
  public string Mode { get; set; } = string.Empty;

  [BindFrom("hub.challenge")]
  public int Challenge { get; set; }

  [BindFrom("hub.verify_token")]
  public string Verify_Token { get; set; } = string.Empty;
}
