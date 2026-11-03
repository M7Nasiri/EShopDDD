using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using EShop.Shared.Application;

namespace _02.Application.ProductAgg.Commands.RemoveProduct;

public class RemoveProductCommandHandler : IBaseCommandHandler<RemoveProductCommand>
{
    private readonly IProductRepository _repository;

    public RemoveProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
    {
        if (request.ProductId == Guid.Empty)
            return OperationResult.NotFound();

        var affectedRows = await _repository.RemoveAsync(request.ProductId, cancellationToken);

        return affectedRows == 0
            ? OperationResult.NotFound()
            : OperationResult.Success();
    }
}