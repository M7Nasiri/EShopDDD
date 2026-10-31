using _01.Domain.Entities.Aggregates.CategoryAgg;
using _02.Application.CategoryAgg.Commands.AddCategory;
using _02.Application.CategoryAgg.Commands.EditCategory;
using _02.Application.CategoryAgg.Commands.RemoveCateogry;
using EShop.Query.CategoryAgg.DTOs;
using EShop.Query.CategoryAgg.GetById;
using EShop.Shared.Application;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EShop.Query.CategoryAgg.GetCategoriesByFilter;
using EShop.Query.CategoryAgg.GetChildCategories;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EShop.Facade.Category
{
    public interface ICategoryFacade
    {
        //Commands
        Task<OperationResult> Add(AddCategoryCommand command);
        Task<OperationResult> Edit(EditCategoryCommand command);
        Task<OperationResult> Delete(RemoveCategoryCommand command);

        //Query
        Task<CategoryDto?> GetById(Guid categoryId);
        Task<CategoryFilterResult> GetByFilter(CategoryFilterParam filterParam);
        Task<List<CategoryDto>> GetChildByParentId(Guid? parentId);
    }

    public class CategoryFacade : ICategoryFacade
    {
        private readonly IMediator _mediator;

        public CategoryFacade(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OperationResult> Add(AddCategoryCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Edit(EditCategoryCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<OperationResult> Delete(RemoveCategoryCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<CategoryDto?> GetById(Guid categoryId)
        {
            return await _mediator.Send(new GetCategoryByIdQuery(categoryId));
        }

        public async Task<CategoryFilterResult> GetByFilter(CategoryFilterParam filterParam)
        {
            return await _mediator.Send(new GetCategoriesByFilterQuery(filterParam));
        }

        public async Task<List<CategoryDto>> GetChildByParentId(Guid? parentId)
        {
            return await _mediator.Send(new GetChildCategoriesByParentIdQuery(parentId));
        }
    }
}
 