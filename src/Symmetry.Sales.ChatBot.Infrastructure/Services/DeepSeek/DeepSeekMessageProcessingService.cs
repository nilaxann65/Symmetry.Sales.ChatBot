using System.Text.Json;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using Symmetry.Sales.ChatBot.Core.ChatAggregate;
using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.Infrastructure.Services.DeepSeek;

internal class DeepSeekMessageProcessingService(
  string apiKey,
  Logger<DeepSeekMessageProcessingService> logger
)
{
  private readonly string ChatModel = "deepseek-chat";

  //private readonly string ReasonerModel = "deepseek-reasoner";
  private readonly int MaxTokensResponse = 300;
  private readonly float ModelTemperature = 1.3F;

  public async Task<Result<string>> ProcessMessageAsync(
    Conversation conversation,
    CancellationToken ct
  )
  {
    List<ChatMessage> messages = [];
    if (!conversation.IsActive)
      return Result.Error("Conversation is closed, can't generate Messages.");

    var chatClient = new ChatClient(ChatModel, apiKey);

    var response = await chatClient.CompleteChatAsync(
      messages,
      new() { MaxOutputTokenCount = MaxTokensResponse, Temperature = ModelTemperature },
      ct
    );

    if (response.Value == null)
    {
      logger.LogError("Error generating error from deepseek");
      return Result.Error("Error generating the message");
    }

    var value = response.Value;
    logger.LogInformation(message: JsonSerializer.Serialize(value));
    return value.Content.FirstOrDefault()?.Text ?? "No content";
  }
}
