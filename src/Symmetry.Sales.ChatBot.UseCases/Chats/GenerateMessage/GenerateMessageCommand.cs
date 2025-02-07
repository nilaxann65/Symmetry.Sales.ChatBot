using Ardalis.Result;
using Ardalis.SharedKernel;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;

namespace Symmetry.Sales.ChatBot.UseCases.Chats.GenerateMessage;

public record GenerateMessageCommand(string UserMessage, string contactId, ChatOrigin chatOrigin)
  : ICommand<Result<string>>;
