using Ardalis.Result;
using Ardalis.SharedKernel;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;
using Symmetry.Sales.ChatBot.Core.ChatAggregate.Specifications;
using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.UseCases.Messages.Generate;

public class GenerateMessageHandler(
  IRepository<Chat> repository,
  IMessageProcessingService messageProcessingService,
  IModels models
) : ICommandHandler<GenerateMessageCommand, Result<string>>
{
  public async Task<Result<string>> Handle(
    GenerateMessageCommand request,
    CancellationToken cancellationToken
  )
  {
    var chat = await repository.FirstOrDefaultAsync(
      new GetChatByContactIdSpec(request.contactId, request.chatOrigin, request.tenantId),
      cancellationToken
    );

    if (chat is null)
      return Result.NotFound("Chat not found");

    chat.AddUserMessage(request.UserMessage);

    var messageCompletionResult = await messageProcessingService.GenerateMessageAsync(
      chat.GetActiveConversation()!,
      models.Chat,
      cancellationToken
    );

    if (!messageCompletionResult.IsSuccess)
      return messageCompletionResult.Map(result => result.Content);

    chat.AddBotMessage(messageCompletionResult.Value.Content);

    await repository.SaveChangesAsync(cancellationToken);

    return Result<string>.Success(messageCompletionResult.Value.Content);
  }
}
