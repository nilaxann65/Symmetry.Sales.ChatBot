using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Symmetry.Sales.ChatBot.UseCases.Companies.Products.GetByDescription;

public record GetByDescriptionCommand(string description) : ICommand<Result<string>>;
