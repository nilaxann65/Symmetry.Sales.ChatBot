using Ardalis.Specification;

namespace Symmetry.Sales.ChatBot.Core.ChatAggregate.Specifications;

public class GetChatByContactIdSpec : Specification<Chat>
{
  public GetChatByContactIdSpec(string contactId, ChatOrigin chatOrigin)
  {
    Query
      .Where(d => d.ContactId == contactId && d.Origin == chatOrigin)
      .Include(d => d.Conversations)
      .ThenInclude(d => d.Messages);
  }
}
