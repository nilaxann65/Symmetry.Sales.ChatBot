using Ardalis.Result;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;
using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.Infrastructure;

public class SemanticKernelService(IChatCompletionService chatCompletionService)
  : IMessageProcessingService
{
  public async Task<Result<Message>> GenerateMessageAsync(
    Conversation conversation,
    string model,
    CancellationToken ct
  )
  {
    var result = await chatCompletionService.GetChatMessageContentAsync(
      Map(conversation),
      new() { ModelId = model },
      cancellationToken: ct
    );

    return result is null || result.Content == string.Empty
      ? Result<Message>.CriticalError($"Error generating message with model {model}")
      : Result<Message>.Success(Map(result));
  }

  public Task<Result<string>> ProcessMessageAsync(Conversation conversation, CancellationToken ct)
  {
    throw new NotImplementedException();
  }

  private ChatHistory Map(Conversation conversation) =>
    new(conversation.Messages.Select(s => new ChatMessageContent(Map(s.Sender), s.Content)));

  private AuthorRole Map(MessageSender sender)
  {
    return sender switch
    {
      MessageSender.User => AuthorRole.User,
      MessageSender.Bot => AuthorRole.Assistant,
      MessageSender.System => AuthorRole.System,
      MessageSender.Function => AuthorRole.Tool,
      _ => AuthorRole.User
    };
  }

  private MessageSender Map(AuthorRole authorRole) =>
    authorRole.Label switch
    {
      "user" => MessageSender.User,
      "assistant" => MessageSender.Bot,
      "system" => MessageSender.System,
      "tool" => MessageSender.Function,
      _ => MessageSender.User
    };

  private Message Map(ChatMessageContent chatMessageContent) =>
    new(chatMessageContent.Content!, Map(chatMessageContent.Role));
}
