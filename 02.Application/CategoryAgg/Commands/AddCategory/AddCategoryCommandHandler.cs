using _01.Domain.Entities.Aggregates.CategoryAgg;
using _01.Domain.Entities.Aggregates.CategoryAgg.Repository;
using _01.Domain.Exceptions;
using _01.Domain.ValueObjects;
using EShop.Shared.Application;
using EShop.Shared.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace _02.Application.CategoryAgg.Commands.AddCategory
{
    public sealed class AddCategoryCommandHandler
     : IBaseCommandHandler<AddCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddCategoryCommandHandler(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationResult> Handle(
            AddCategoryCommand request,
            CancellationToken cancellationToken)
        {
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

            var category = new Category(
                name,
                parentCategoryId);

            await _categoryRepository.AddAsync(
                category,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);

            return OperationResult.Success();
        }
    }
}
