using Symmetry.Sales.ChatBot.Core.Interfaces;

namespace Symmetry.Sales.ChatBot.Infrastructure.Services.ChatModels;

public class Models : IModels
{
  public string Reasoner => "reasoner";
  public string Chat => "chat";
  public string WeakChat => "weakchat";
}
