using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Symmetry.Sales.ChatBot.UseCases.Contributors.Get;
public record GetContributorQuery(int ContributorId) : IQuery<Result<ContributorDTO>>;
