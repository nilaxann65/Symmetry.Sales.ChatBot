using Ardalis.Result;

namespace Symmetry.Sales.ChatBot.Core.Interfaces;

public interface IMessagingService
{
  public Task<Result> SendTextMessageAsync(
    string fromId,
    string destination,
    string message,
    bool previewUrl,
    CancellationToken cancellationToken
  );
}
