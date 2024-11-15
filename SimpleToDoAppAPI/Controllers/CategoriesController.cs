using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SharedDtoModels.CategoryDtos;
using SimpleToDoAppAPI.Services;

namespace SimpleToDoAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // Create a Category
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto category)
        {
            
            // Check if the categoryDto object sent in the request is valid
            if (!ModelState.IsValid)
            {
                // Checks if the invalid state is because of the Summary property, which should be allowed
                if (!string.IsNullOrEmpty(ModelState["Summary"]?.AttemptedValue))
                    return BadRequest(ModelState);
            }

            // If the model is valid, add the category
            await _categoryService.AddAsync(category);

            return Ok(new
            {
                message = "New category added.",
                categoryDetails = category
            });
        }

        // Get all categories
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllAsync();

            // Check if there are any elements queried at all
            if (!categories.Any())
            {
                return NotFound(new
                {
                    message = "There are no categories found."
                });
            }

            // If there are elements listed, return Ok
            return Ok(new
            {
                message = "Categories successfully listed.",
                categories
            });
        }

    }
}
