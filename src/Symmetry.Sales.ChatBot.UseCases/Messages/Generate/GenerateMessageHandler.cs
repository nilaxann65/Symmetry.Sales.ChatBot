using Ardalis.Result;
using Ardalis.SharedKernel;
using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.UseCases.Messages.Generate;

public class GenerateMessageHandler(
  IMessageProcessingService messageProcessingService,
  IModels models
) : ICommandHandler<GenerateMessageCommand, Result<string>>
{
  public async Task<Result<string>> Handle(
    GenerateMessageCommand request,
    CancellationToken cancellationToken
  )
  {
    var messageCompletionResult = await messageProcessingService.GenerateMessageAsync(
      request.chat.GetActiveConversation()!,
      models.Chat,
      cancellationToken
    );

    if (!messageCompletionResult.IsSuccess)
      return messageCompletionResult.Map(result => result.Content);

    return Result<string>.Success(messageCompletionResult.Value.Content);
  }
}
