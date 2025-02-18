using Ardalis.Result;
using Ardalis.SharedKernel;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;

namespace Symmetry.Sales.ChatBot.UseCases.Messages.Generate;

public record GenerateMessageCommand(Chat chat) : ICommand<Result<string>>;
