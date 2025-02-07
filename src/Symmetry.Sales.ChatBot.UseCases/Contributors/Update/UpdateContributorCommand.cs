using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Symmetry.Sales.ChatBot.UseCases.Contributors.Update;
public record UpdateContributorCommand(int ContributorId, string NewName) : ICommand<Result<ContributorDTO>>;
