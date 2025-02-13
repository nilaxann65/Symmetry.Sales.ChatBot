using Ardalis.Specification;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.Core.ChatAggregate.Specifications;

public class GetChatByContactIdSpec : Specification<Chat>
{
  public GetChatByContactIdSpec(string contactId, Channel channel, int tenantId)
  {
    Query
      .Where(d => d.ContactId == contactId && d.Origin == channel && d.TenantId == tenantId)
      .Include(d => d.Conversations)
      .ThenInclude(d => d.Messages);
  }
}
