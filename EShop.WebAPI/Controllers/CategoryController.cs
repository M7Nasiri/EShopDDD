using _02.Application.CategoryAgg.Commands.AddCategory;
using _02.Application.CategoryAgg.Commands.EditCategory;
using _02.Application.CategoryAgg.Commands.RemoveCateogry;
using EShop.Facade.Category;
using EShop.Query.CategoryAgg.DTOs;
using EShop.Shared.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryFacade _categoryFacade;

        public CategoryController(ICategoryFacade categoryFacade)
        {
            _categoryFacade = categoryFacade;
        }

        [HttpPost]
        public async Task<OperationResult> CreateCategory(AddCategoryCommand command)
        {
            return await _categoryFacade.Add(command);
        }

        [HttpPut]
        public async Task<OperationResult> EditCategory(EditCategoryCommand command)
        {
            return await _categoryFacade.Edit(command);
        }

        [HttpPost("Delete")]
        public async Task<OperationResult> DeleteCategory(RemoveCategoryCommand command)
        {
            return await _categoryFacade.Delete(command);
        }

        [HttpGet("{categoryId}")]
        public async Task<CategoryDto?> GetById(Guid categoryId)
        {
            return await _categoryFacade.GetById(categoryId);
        }

        [HttpGet("getByFilter")]
        public async Task<CategoryFilterResult> GetByFilter(CategoryFilterParam filterParam)
        {
            return await _categoryFacade.GetByFilter(filterParam);
        }

        [HttpGet("getChild/{parentId}")]
        public async Task<List<CategoryDto>> GetChildren(Guid? parentId)
        {
            return await _categoryFacade.GetChildByParentId(parentId);
        }

    }
}
