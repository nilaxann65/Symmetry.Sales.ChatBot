using Ardalis.Result;
using Ardalis.SharedKernel;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;
using Symmetry.Sales.ChatBot.Core.ChatAggregate.Specifications;
using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.UseCases.Chats.StartChat;

public class StartChatHandler(
  IRepository<Chat> repository,
  IMessageProcessingService messageProcessingService,
  IModels models
) : ICommandHandler<StartChatCommand, Result<string>>
{
  public async Task<Result<string>> Handle(
    StartChatCommand request,
    CancellationToken cancellationToken
  )
  {
    var chat = new Chat(request.chatOrigin, request.contactId, request.tenantId);

    chat.InitConversation(request.UserMessage);

    var messageCompletionResult = await messageProcessingService.GenerateMessageAsync(
      chat.GetActiveConversation()!,
      models.Chat,
      cancellationToken
    );

    if (!messageCompletionResult.IsSuccess)
      return messageCompletionResult.Map(result => result.Content);

    chat.AddBotMessage(messageCompletionResult.Value.Content);

    await repository.AddAsync(chat, cancellationToken);

    return Result<string>.Success(messageCompletionResult.Value.Content);
  }
}
