using Ardalis.Specification;

namespace Symmetry.Sales.ChatBot.Core.ContributorAggregate.Specifications;
public class ContributorByIdSpec : Specification<Contributor>
{
  public ContributorByIdSpec(int contributorId)
  {
    Query
        .Where(contributor => contributor.Id == contributorId);
  }
}
