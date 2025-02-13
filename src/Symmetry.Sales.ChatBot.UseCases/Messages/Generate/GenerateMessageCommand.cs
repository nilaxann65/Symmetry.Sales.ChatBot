using Ardalis.Result;
using Ardalis.SharedKernel;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.UseCases.Messages.Generate;

public record GenerateMessageCommand(
  string UserMessage,
  string contactId,
  Channel chatOrigin,
  int tenantId
) : ICommand<Result<string>>;
