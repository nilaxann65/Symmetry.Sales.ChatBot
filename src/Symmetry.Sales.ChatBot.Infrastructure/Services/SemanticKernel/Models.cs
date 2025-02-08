using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.Infrastructure.Services.SemanticKernel;

public class Models : IModels
{
  public string Reasoner => "gemini-1.5-flash-8b";
  public string Chat => "gemini-1.5-flash-8b";
  public string WeakChat => "gemini-1.5-flash-8b";
}
