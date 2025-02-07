using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Symmetry.Sales.ChatBot.UseCases.Contributors.Delete;
public record DeleteContributorCommand(int ContributorId) : ICommand<Result>;
