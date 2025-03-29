using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.Infrastructure.Services.SemanticKernel;

public class Models : IModels
{
  public string Reasoner => "gemini-1.5-flash-8b";
  public string Chat => "gemini-1.5-flash-8b";
  public string WeakChat => "gemini-1.5-flash-8b";
}

#pragma warning disable SKEXP0070
public static class ModelsExtension
{
  public static void AddModels(this IServiceCollection services)
  {
    services.AddKeyedSingleton<PromptExecutionSettings, GeminiPromptExecutionSettings>(
      "Polite",
      (sp, key) =>
      {
        return new GeminiPromptExecutionSettings()
        {
          ModelId = "gemini-2.0-flash-lite",
          ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions,
          Temperature = 1.5,
          MaxTokens = 1000,
        };
      }
    );

    services.AddKeyedSingleton<PromptExecutionSettings, GeminiPromptExecutionSettings>(
      "Analytic",
      (sp, key) =>
      {
        return new GeminiPromptExecutionSettings()
        {
          ModelId = "gemini-2.0-flash",
          ToolCallBehavior = GeminiToolCallBehavior.AutoInvokeKernelFunctions,
          Temperature = 0.5,
          MaxTokens = 1000,
        };
      }
    );
  }
}

#pragma warning restore SKEXP0070
