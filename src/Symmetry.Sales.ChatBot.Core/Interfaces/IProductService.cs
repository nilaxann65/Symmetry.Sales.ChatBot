using Ardalis.Result;
using Symmetry.Sales.ChatBot.Core.BusinessAggregate;

namespace Symmetry.Sales.ChatBot.Core.Interfaces;

public interface IProductService
{
  public Task<Result<IProduct>> GetProductByIdAsync(Guid id, CancellationToken ct = default);
  public Task<Result> AddProductAsync(
    Guid id,
    string name,
    string description,
    double price,
    string[] tags,
    CancellationToken ct = default
  );
  public Task<Result> DeleteProductByIdAsync(Guid id, CancellationToken ct = default);

  public Task<Result<IProduct>> GetProductByDescriptionAsync(
    string description,
    CancellationToken ct = default
  );
}
