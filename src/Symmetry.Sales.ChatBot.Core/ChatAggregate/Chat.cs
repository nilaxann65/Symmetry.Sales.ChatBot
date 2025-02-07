using Ardalis.SharedKernel;

namespace Symmetry.Sales.ChatBot.Core.ChatAggregate;

public class Chat : EntityBase, IAggregateRoot
{
  public ChatOrigin Origin { get; private set; }
  public string ContactId { get; private set; }
  public List<Conversation> Conversations { get; private set; } = [];

  private Chat()
  {
    ContactId = string.Empty;
  }

  public Chat(ChatOrigin origin, string contactId)
  {
    Origin = origin;
    ContactId = contactId;
  }

  public void InitConversation(string userMessage)
  {
    foreach (var conversation in Conversations.Where(d => d.IsActive))
    {
      conversation.CloseConversation();
    }

    Conversations.Add(new Conversation(userMessage));
  }

  public Conversation? GetActiveConversation()
  {
    return Conversations.Find(d => d.IsActive);
  }

  public void AddBotMessage(string content)
  {
    var conversation = GetActiveConversation();
    conversation?.AddBotMessage(content);
  }

  public void AddUserMessage(string content)
  {
    var conversation = GetActiveConversation();
    conversation?.AddUserMessage(content);
  }
}
