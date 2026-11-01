using _01.Domain.Entities.Aggregates.CategoryAgg;
using _02.Application.CategoryAgg.Commands.AddCategory;
using _02.Application.CategoryAgg.Commands.EditCategory;
using _02.Application.CategoryAgg.Commands.RemoveCateogry;
using _02.Application.CommentAgg.Commands.AddComment;
using EShop.Facade.Category;
using EShop.Query.CategoryAgg.DTOs;
using EShop.Query.CommentAgg.DTOs.Admin;
using EShop.Shared.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryFacade _categoryFacade;

        public CategoriesController(ICategoryFacade categoryFacade)
        {
            _categoryFacade = categoryFacade;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody]AddCategoryCommand command)
        {
            var result =  await _categoryFacade.Add(command);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpPut("{categoryId:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit(Guid categoryId, 
            [FromBody] EditCategoryRequest request)
        {
            var command = new EditCategoryCommand(
                categoryId,
                request.Name,
                request.ParentCategoryId);

            var result = await _categoryFacade.Edit(command);
            return Ok(result);
        }

        [HttpDelete("{categoryId:guid}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            var result = await _categoryFacade.Delete(new RemoveCategoryCommand(categoryId));
            return Ok(result);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDto?>> GetById(Guid categoryId)
        {
            var result = await _categoryFacade.GetById(categoryId);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CategoryFilterResult), StatusCodes.Status200OK)]
        public async Task<ActionResult<CategoryFilterResult>> GetByFilter(
            [FromQuery]CategoryFilterParam filterParam)
        {
            var result = await _categoryFacade.GetByFilter(filterParam);
            return Ok(result);
        }

        [HttpGet("roots")]
        [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CategoryDto>>> GetRootCategories()
        {
            var result = await _categoryFacade.GetChildByParentId(null);
            return Ok(result);
        }

        [HttpGet("{parentId:guid?}/children")]
        [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<CategoryDto>>> GetChildren(Guid parentId)
        {
            var result = await _categoryFacade.GetChildByParentId(parentId);
            return Ok(result); 
        }

    }
    public sealed record EditCategoryRequest(string Name, Guid? ParentCategoryId);
}
