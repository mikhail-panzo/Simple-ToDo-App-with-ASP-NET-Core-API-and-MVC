using Microsoft.AspNetCore.Mvc;
using SimpleToDoAppAPI.Models;
using SharedModels.CategoryDtos;
using SimpleToDoAppAPI.Services;
using SharedModels.Responses;
using Newtonsoft.Json;
using SharedModels.TaskDtos;

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

        /// <summary>
        /// Create a Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto category)
        {
            
            // Check if the categoryDto object sent in the request is valid
            if (!ModelState.IsValid)
            {
                // Checks if the invalid state is because of the Summary property, which should be allowed
                if (!string.IsNullOrEmpty(ModelState["Summary"]?.AttemptedValue))
                    // Return a bad request with details of the invalid category data
                    return BadRequest(ModelState);
            }

            // Get an existing category if the name already exists, then return a bad request if it is
            Category? existingCategory = await _categoryService.GetByNameAsync(category.Name);
            if(existingCategory != null)
            {
                return BadRequest(new DataResponse<CategoryWithIdDto>
                {
                    Message = "Category with the name already exists.",
                    Data = existingCategory.ToDtoWithId()
                });
            }

            // If the model is valid, try to add it
            if(await _categoryService.AddAsync(category))
            {
                return Ok(new DataResponse<CategoryWithIdDto>
                {
                    Message = "New category created.",
                    Data = (await _categoryService.GetByNameAsync(category.Name))?.ToDtoWithId()
                    ?? throw new Exception("Category not found after being added.")
                });
            }
               
            // If the model cannot be saved
            return Problem(
                detail: "Adding the category failed.",
                statusCode: 500,
                title: "Internal Server Error");
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            List<Category> categories = await _categoryService.GetAllAsync();

            // Check if there are any elements queried at all
            if (!categories.Any())
            {
                return NotFound(new DataResponse<List<CategoryWithIdDto>>
                {
                    Message = $"There are no categories.",
                    Data = null
                });
            }

            // If there are elements listed, return an Ok response with the categories
            return Ok(new DataResponse<List<CategoryWithIdDto>>
            {
                Message = "Categories retrieved successfully.",
                Data = categories.Select(c => c.ToDtoWithId()).ToList()
            });
        }

        /// <summary>
        /// Get a category based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetCategory([FromRoute] long id)
        {
            Category? category = await _categoryService.GetByIdAsync(id);

            // If there is no existing category based on the id, return a not found request
            if (category == null)
            {
                return NotFound(new DataResponse<CategoryWithIdDto>
                {
                    Message = $"Category with the ID of {id} does not exists.",
                    Data = null
                });
            }

            // If the category exists, return it
            return Ok(new DataResponse<CategoryWithIdDto>
            {
                Message = "Category retreived successfully.",
                Data = category.ToDtoWithId()
            });
        }

        /// <summary>
        /// Updates a Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPut("{id:long}")]
        public async Task<IActionResult> PutCategory([FromRoute] long id, [FromBody] CategoryDto category)
        {
            Category? existingCategory = await _categoryService.GetByIdAsync(id);

            // If there is no existing category based on the id, return a not found request
            if (existingCategory == null)
            {
                return StatusCode(498, new DataResponse<CategoryWithIdDto>
                {
                    Message = $"Category with the ID of {id} does not exists.",
                    Data = null
                });
            }

            // If the category being sent and the existing category is the same
            if (JsonConvert.SerializeObject(existingCategory.ToDto()) == JsonConvert.SerializeObject(category))
            {
                return StatusCode(497, new DataResponse<CategoryWithIdDto>
                {
                    Message = "Category being updated is not modified.",
                    Data = null
                });
            }

            // Check if there is a conflicted category, that is a category with the same name
            Category? conflictCategory = await _categoryService.GetByNameAsync(category.Name);
            if(conflictCategory != null)
            {
                return StatusCode(496, new DataResponse<CategoryWithIdDto>
                {
                    Message = "Category with the name already exists.",
                    Data = existingCategory.ToDtoWithId()
                });
            }

            // Perform an update and check if it was successful
            if(await _categoryService.UpdateAsync(id, category))
                return Ok(new DataResponse<CategoryWithIdDto>
                {
                    Message = $"Category with the ID of {id} successfully updated.",
                    Data = null
                });

            // If the update was unsuccessful and the existingCategory no longer exists
            return NotFound(new DataResponse<CategoryWithIdDto>
            {
                Message = "Concurrency Error: Category being updated does not exists during the update.",
                Data = null
            });
        }

        /// <summary>
        /// Deletes a Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] long id)
        {
            Category? existingCategory = await _categoryService.GetByIdAsync(id);

            // If there is no existing category based on the id, return a not found request
            if (existingCategory == null)
            {
                return BadRequest(new DataResponse<List<ToDoTaskDisplayDto>>
                {
                    Message = $"Category with the ID of {id} does not exists.",
                    Data = null
                });
            }

            // If there are tasks assigned to the category, return a bad request
            // This type of validation should be done if an entity is a foreign key of another and has the constraint of not being deleted
            if (_categoryService.TryGetTasks(id, out List<ToDoTaskDisplayDto> tasks))

                // Set a custom status code for bad requests where a category has tasks
                return StatusCode(499, new DataResponse<List<ToDoTaskDisplayDto>>
                {
                    Message = "Category has tasks assigned to it.",
                    Data = tasks
                });

            // If the category exists, delete it
            if(await _categoryService.DeleteAsync(existingCategory))
                return Ok(new DataResponse<List<ToDoTaskDisplayDto>>
                { 
                    Message = $"Category with the ID of {id} deleted successfully.",
                    Data = null
                });

            // If the model cannot be deleting
            return Problem(
                detail: "Deleting the category failed.",
                statusCode: 500,
                title: "Internal Server Error");
        }
    }
}
