using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.ChatCompletion;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;
using Symmetry.Sales.ChatBot.Core.ChatAggregate.Specifications;
using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.UseCases.Chats.GenerateMessage;

public class GenerateMessageHandler(
  IRepository<Chat> repository,
  IChatCompletionService chatCompletionService,
  IModels models
) : ICommandHandler<GenerateMessageCommand, Result<string>>
{
  public async Task<Result<string>> Handle(
    GenerateMessageCommand request,
    CancellationToken cancellationToken
  )
  {
    var chat = await repository.FirstOrDefaultAsync(
      new GetChatByContactIdSpec(request.contactId, request.chatOrigin),
      cancellationToken
    );

    chat ??= new Chat(request.chatOrigin, request.contactId);

    var chatHistory = new ChatHistory();
    chatHistory.AddUserMessage("testeando ando");

    var result = await chatCompletionService.GetChatMessageContentsAsync(
      chatHistory,
      new() { ModelId = models.Chat, },
      cancellationToken: cancellationToken
    );

    if (chat.GetActiveConversation() is null)
      chat.InitConversation(request.UserMessage);
    else
      chat.AddUserMessage(request.UserMessage);

    return Result<string>.Success("");
  }
}
