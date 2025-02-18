using Ardalis.Result;
using Ardalis.SharedKernel;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.UseCases.Chats.AnswerMessage;

public record AnswerMessageCommand(
  string sender,
  string destinataryId,
  Channel channel,
  string userMessage
) : ICommand<Result>;
