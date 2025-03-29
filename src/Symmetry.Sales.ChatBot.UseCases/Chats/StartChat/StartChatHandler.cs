using Ardalis.Result;
using Ardalis.SharedKernel;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;
using Symmetry.Sales.ChatBot.Core.ChatAggregate.Specifications;
using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.UseCases.Chats.StartChat;

public class StartChatHandler(IMessageProcessingService messageProcessingService)
  : ICommandHandler<StartChatCommand, Result<Chat>>
{
  public async Task<Result<Chat>> Handle(
    StartChatCommand request,
    CancellationToken cancellationToken
  )
  {
    var chat = new Chat(request.chatOrigin, request.contactId, request.tenantId);

    chat.InitConversation(request.UserMessage);

    var messageCompletionResult = await messageProcessingService.GenerateMessageAsync(
      chat.GetActiveConversation()!,
      cancellationToken
    );

    if (!messageCompletionResult.IsSuccess)
      return messageCompletionResult.Map(result => new Chat(default, string.Empty, default));

    chat.AddBotMessage(messageCompletionResult.Value.Content);

    return Result.Success(chat);
  }
}
