using Ardalis.Result;
using Ardalis.SharedKernel;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;

namespace Symmetry.Sales.ChatBot.UseCases.Chats.StartChat;

public record StartChatCommand(
  string UserMessage,
  string contactId,
  Channel chatOrigin,
  int tenantId
) : ICommand<Result<Chat>>;
