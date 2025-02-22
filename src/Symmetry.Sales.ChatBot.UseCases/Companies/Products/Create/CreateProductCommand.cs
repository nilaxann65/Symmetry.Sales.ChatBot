using Ardalis.Result;
using Ardalis.SharedKernel;

namespace Symmetry.Sales.ChatBot.UseCases.Companies.Products.Create;

public record CreateProductCommand(string name, string description, double price, string[] tags)
  : ICommand<Result<Guid>>;
