using System.Security.Cryptography.X509Certificates;
using Ardalis.Specification;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.Core.ChatAggregate.Specifications;

public class GetChatByContactIdSpec : Specification<Chat>
{
  public GetChatByContactIdSpec(string contactId, Channel channel)
  {
    Query
      .Where(d => d.ContactId == contactId && d.Origin == channel)
      .Include(d => d.Conversations.Where(x => x.IsActive))
      .ThenInclude(d => d.Messages);
  }
}
