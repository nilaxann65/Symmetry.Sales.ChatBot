using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using SemanticFlow.Interfaces;
using SemanticFlow.Services;

namespace Symmetry.Sales.ChatBot.Core.Services.Workflows.Common;

public class CustomerIdentificationActivity(
  ILogger<CustomerIdentificationActivity> logger,
  WorkflowService workflowService,
  Kernel kernel,
  [FromKeyedServices("Polite")] PromptExecutionSettings promptExecutionSettings
) : IActivity
{
  public string SystemPrompt { get; set; } =
    "Estas atendiendo una tienda de abarrotes a travez del chat de la empresa con los clientes. /n Tu tarea es identificar al cliente, debes pedir su nombre, no importa si es un alias";
  public PromptExecutionSettings PromptExecutionSettings { get; set; } = promptExecutionSettings;

  [
    KernelFunction("customer_identification_approved"),
    Description("Confirms the customer data for the order")
  ]
  public string ExecuteAsync()
  {
    string chatId = "1";
    var nextActivity = workflowService.CompleteActivity(chatId, kernel);

    var result =
      @$"{nextActivity!.SystemPrompt} ###
                      {workflowService.WorkflowState.DataFrom(chatId).ToPromptString()}";

    logger.LogInformation(result);

    return result;
  }
}
