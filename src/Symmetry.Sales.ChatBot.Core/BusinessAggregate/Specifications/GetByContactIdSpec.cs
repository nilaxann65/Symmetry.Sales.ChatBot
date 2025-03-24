using Ardalis.Specification;

namespace Symmetry.Sales.ChatBot.Core.BusinessAggregate.Specifications;

public class GetByContactIdSpec : SingleResultSpecification<Business>
{
  public GetByContactIdSpec(string contactId, Channel channel)
  {
    Query
      .Include(s => s.Contacts)
      .Include(s => s.PaymentMethods)
      .Where(s => s.Contacts.Any(c => c.ContactId == contactId && c.ContactOrigin == channel));
  }
}
