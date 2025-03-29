using Ardalis.SharedKernel;

namespace Symmetry.Sales.ChatBot.Core.ChatAggregate;

public class Conversation : EntityBase
{
  public List<Message> Messages { get; private set; } = [];
  public bool IsActive { get; private set; } = true;

  public Conversation(string userMessage)
  {
    string systemInstructions =
      "Actuaras como un agente de ventas de una tienda de frutas y prendas de vestir. \n se conciso, amable y directo con tus respuestas pero no cortante. Toda la conversacion sera en español.";

    Messages.AddRange(
      [
        new Message(systemInstructions, MessageSender.System),
        new Message(userMessage, MessageSender.User),
      ]
    );
  }

  public void ModifySystemInstructions(string instructions)
  {
    var systemMessageIndex = Messages.FindIndex(d => d.Sender == MessageSender.System);
    if (systemMessageIndex != -1)
    {
      Messages[systemMessageIndex] = new Message(instructions, MessageSender.System);
    }
  }

  public string GetBotGeneratedMessage() =>
    Messages.LastOrDefault(d => d.Sender == MessageSender.Bot)?.Content ?? string.Empty;

  public void AddBotMessage(string content) =>
    Messages.Add(new Message(content, MessageSender.Bot));

  public void AddUserMessage(string content) =>
    Messages.Add(new Message(content, MessageSender.User));

  public void CloseConversation() => IsActive = false;

  // Required for EF
  private Conversation() { }
}
