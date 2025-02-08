using Ardalis.SharedKernel;

namespace Symmetry.Sales.ChatBot.Core.ChatAggregate;

public class Message : EntityBase
{
  public DateTime Date { get; private set; }
  public string Content { get; private set; }
  public MessageSender Sender { get; private set; }

  public Message(string content, MessageSender sender)
  {
    Date = DateTime.Now;
    Content = content;
    Sender = sender;
  }

  private Message()
  {
    Content = string.Empty;
  }
}

public enum MessageSender
{
  User,
  Bot,
  System,
  Function
}
