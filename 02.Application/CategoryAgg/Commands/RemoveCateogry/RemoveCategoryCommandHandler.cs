using _01.Domain.Entities.Aggregates.CategoryAgg.Repository;
using _01.Domain.Entities.Aggregates.ProductAgg.Repository;
using _01.Domain.Exceptions;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CategoryAgg.Commands.RemoveCateogry
{
    public sealed class RemoveCategoryCommandHandler
      : IBaseCommandHandler<RemoveCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> Handle(
            RemoveCategoryCommand request,
            CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetTracking(
                request.CategoryId,
                cancellationToken);

            if (category is null)
            {
                throw new EShopNullException(
                    "Category was not found.");
            }

            var hasChildren = await _categoryRepository
                .HasChildrenAsync(
                    category.Id,
                    cancellationToken);

            var hasProducts = await _productRepository
                .ExistsByCategoryIdAsync(
                    category.Id.Value,
                    cancellationToken);

            category.EnsureCanBeDeleted(
                hasChildren,
                hasProducts);

            // حذف فیزیکی از DbContext
            category.SoftDelete();

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return OperationResult.Success();
        }
    }
}
