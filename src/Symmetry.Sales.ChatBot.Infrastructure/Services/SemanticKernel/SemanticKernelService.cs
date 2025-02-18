using Ardalis.Result;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Google;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;
using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.Infrastructure.Services.SemanticKernel;

public class SemanticKernelService( /*IChatCompletionService chatCompletionService,*/
  Kernel kernel
) : IMessageProcessingService
{
  public async Task<Result<Message>> GenerateMessageAsync(
    Conversation conversation,
    string model,
    CancellationToken ct
  )
  {
    try
    {
      var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

      var request = Map(conversation);
#pragma warning disable SKEXP0070 // Este tipo se incluye solo con fines de evaluación y está sujeto a cambios o a que se elimine en próximas actualizaciones. Suprima este diagnóstico para continuar.
      GeminiPromptExecutionSettings geminiAIPromptExecutionSettings =
        new()
        {
          ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions,
          ModelId = model
        };
#pragma warning restore SKEXP0070 // Este tipo se incluye solo con fines de evaluación y está sujeto a cambios o a que se elimine en próximas actualizaciones. Suprima este diagnóstico para continuar.

      var result = await chatCompletionService.GetChatMessageContentAsync(
        request,
        geminiAIPromptExecutionSettings,
        kernel,
        cancellationToken: ct
      );

      return result is null || result.Content == string.Empty
        ? Result<Message>.CriticalError($"Error generating message with model {model}")
        : Result<Message>.Success(Map(result));
    }
    catch (Exception ex)
    {
      return new Message(ex.Message, MessageSender.Bot);
    }
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
