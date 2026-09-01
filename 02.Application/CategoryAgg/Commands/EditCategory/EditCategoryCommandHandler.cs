using _01.Domain.Entities.Aggregates.CategoryAgg;
using _01.Domain.Entities.Aggregates.CategoryAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CategoryAgg.Commands.EditCategory
{
    public class EditCategoryCommandHandler : IBaseCommandHandler<EditCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EditCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetTracking(request.CategoryId,cancellationToken);
            if (category is null)
                throw new EShopNullException("Category is null");

           var name = new Name(request.Name);

            _01.Domain.Guid? parentCategoryId = null;

            if (request.ParentCategoryId.HasValue)
            {
                var parentExists = await _categoryRepository.ExistsAsync(
                    request.ParentCategoryId.Value,
                    cancellationToken);

                if (!parentExists)
                {
                    throw new EShopNullException(
                        "Parent category was not found.");
                }

                parentCategoryId = new Guid(
                    request.ParentCategoryId.Value);
            }

            if (category.ParentCategoryId == parentCategoryId)
                throw new EShopDomainException("Category cannot be it's own parent");

            category.ChangeParent(parentCategoryId);
            category.ChangeName(name);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return OperationResult.Success();
        }
    }
}
