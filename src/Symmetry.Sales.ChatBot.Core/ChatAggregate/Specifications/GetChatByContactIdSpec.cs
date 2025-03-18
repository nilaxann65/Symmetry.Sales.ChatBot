using Ardalis.Specification;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.Core.ChatAggregate.Specifications;

public class GetChatByContactIdSpec : Specification<Chat>
{
  public GetChatByContactIdSpec(string contactId, Channel channel)
  {
    Query
      .Where(d => d.ContactId == contactId && d.Origin == channel)
      .Include(d => d.Conversations)
      .ThenInclude(d => d.Messages);
  }
}
