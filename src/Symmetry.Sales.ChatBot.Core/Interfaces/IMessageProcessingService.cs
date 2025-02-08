using Ardalis.Result;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;

namespace Symmetry.Sales.ChatBot.Core.Interfaces;

public interface IMessageProcessingService
{
  public Task<Result<string>> ProcessMessageAsync(Conversation conversation, CancellationToken ct);

  public Task<Result<Message>> GenerateMessageAsync(
    Conversation conversation,
    string model,
    CancellationToken ct
  );
}
