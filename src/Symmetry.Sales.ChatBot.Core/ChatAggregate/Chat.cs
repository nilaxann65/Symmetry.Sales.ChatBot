using Ardalis.SharedKernel;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.Core.ChatAggregate;

public class Chat : EntityBase, IAggregateRoot
{
  public int TenantId { get; private set; }
  public Channel Origin { get; private set; }
  public string ContactId { get; private set; }
  public List<Conversation> Conversations { get; private set; } = [];

  public Chat(Channel origin, string contactId, int tenantId)
  {
    Origin = origin;
    ContactId = contactId;
    TenantId = tenantId;
  }

  public bool HasActiveConversation() => Conversations.Any(d => d.IsActive);

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

  public string GetBotGeneratedMessage()
  {
    var conversation = GetActiveConversation();
    return conversation!.GetBotGeneratedMessage();
  }

  // Required for EF
  private Chat()
  {
    ContactId = string.Empty;
  }
}
