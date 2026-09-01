using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.ProductAgg.Commands.ChangePrice
{
    public class ChangePriceCommandHandler : IBaseCommandHandler<ChangePriceCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChangePriceCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult> Handle(ChangePriceCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetAsync(request.ProductId, cancellationToken);
            if (product is null)
                throw new EShopDomainException("کالای مورد نظر یافت نشد.");

            product.ChangePrice(new Money(request.NewPrice));

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return OperationResult.Success();
        }
    }
}
