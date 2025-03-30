using Ardalis.Result;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using SemanticFlow.Extensions;
using SemanticFlow.Services;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;
using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.Infrastructure.Services.SemanticKernel;

public class SemanticKernelService(Kernel kernel, ILogger<SemanticKernelService> logger)
  : IMessageProcessingService
{
  public async Task<Result<Message>> GenerateMessageAsync(
    Conversation conversation,
    CancellationToken ct
  )
  {
    try
    {
      var chatId = "1";

      var workflowService = kernel.GetRequiredService<WorkflowService>();

      var currentActivity = workflowService.GetCurrentActivity(chatId, kernel)!;

      logger.LogInformation(
        "Generating message with model {model} for activity {activity}",
        currentActivity.PromptExecutionSettings.ModelId,
        currentActivity.GetType().Name
      );

      var systemPrompt =
        currentActivity.SystemPrompt
        + " ### "
        + workflowService.WorkflowState.DataFrom(chatId).ToPromptString();

      conversation.ModifySystemInstructions(systemPrompt);

      var request = Map(conversation);

      var chatCompletion = kernel.GetChatCompletionForActivity(currentActivity);

      var result = await chatCompletion.GetChatMessageContentAsync(
        request,
        currentActivity.PromptExecutionSettings,
        kernel,
        ct
      );

      if (result is null || result.Content == string.Empty)
        return Result<Message>.CriticalError(
          $"Error generating message with model {currentActivity.PromptExecutionSettings.ModelId}"
        );

      logger.LogInformation(
        "Message generated successfully, {tokens} tokens used",
        result.Metadata?.FirstOrDefault(x => x.Key == "TotalTokenCount").Value?.ToString()
      );

      return Result<Message>.Success(Map(result));
    }
    catch (Exception ex)
    {
      return new Message(ex.Message, MessageSender.Bot);
    }
  }

  private ChatHistory Map(Conversation conversation) =>
    [.. conversation.Messages.Select(s => new ChatMessageContent(Map(s.Sender), s.Content))];

  private AuthorRole Map(MessageSender sender)
  {
    return sender switch
    {
      MessageSender.User => AuthorRole.User,
      MessageSender.Bot => AuthorRole.Assistant,
      MessageSender.System => AuthorRole.System,
      MessageSender.Function => AuthorRole.Tool,
      _ => AuthorRole.User,
    };
  }

  private MessageSender Map(AuthorRole authorRole) =>
    authorRole.Label switch
    {
      "user" => MessageSender.User,
      "assistant" => MessageSender.Bot,
      "system" => MessageSender.System,
      "tool" => MessageSender.Function,
      _ => MessageSender.User,
    };

  private Message Map(ChatMessageContent chatMessageContent) =>
    new(chatMessageContent.Content!, Map(chatMessageContent.Role));
}
