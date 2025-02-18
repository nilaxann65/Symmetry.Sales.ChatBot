using Ardalis.Result;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;

namespace Symmetry.Sales.ChatBot.Core.Interfaces;

public interface IMessageProcessingService
{
  public Task<Result<Message>> GenerateMessageAsync(
    Conversation conversation,
    string model,
    CancellationToken ct
  );
}
