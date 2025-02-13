using FastEndpoints;

namespace Symmetry.Sales.ChatBot.Web.WebHooks.Meta;

public class Verify : Endpoint<VerifyRequest, int>
{
  public override void Configure()
  {
    Get(VerifyRequest.Route);
    AllowAnonymous();
    Summary(s => s.Summary = "Verify the webhook with meta");
  }

  public override async Task HandleAsync(VerifyRequest req, CancellationToken ct)
  {
    if (req.Verify_Token == "1234")
      await SendOkAsync(req.Challenge, ct);
    else
      await SendForbiddenAsync(cancellation: ct);
  }
}
