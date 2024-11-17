using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SharedModels.CategoryDtos;
using SharedModels.Responses;
using SharedModels.TaskDtos;
using SimpleToDoAppWebMVC.Services;
using System.Text.Json;

namespace SimpleToDoAppWebMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // The index page is where you'll view all the categories
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GetAllAsync());
        }

        // The Create page is where you'll create a new category
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // This is the post method for the create page
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind] CategoryDto categoryDto)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Post the category
            HttpResponseMessage response = await _categoryService.CreateAsync(categoryDto);

            // Check if the response is successful
            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            // If the response is not successful, show the error
            ModelState.AddModelError("CreateError", "Something went wrong while creating the category.");
            return View();
        }

        // The Update page is where you'll update an existing category
        [HttpGet]
        public async Task<IActionResult> Update([FromRoute] long id)
        {
            CategoryWithIdDto? category = await _categoryService.GetAsync(id);

            if (category == null)
            {
                TempData["Errors"] = new string[] { "Category not found." };
                return RedirectToAction(nameof(Index));
            }

            return View(await _categoryService.GetAsync(id));
        }

        // This is the post method for updating an existing category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] long id, [Bind] CategoryWithIdDto category)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Attempt to update the category
            HttpResponseMessage response = await _categoryService.UpdateAsync(id, category);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Category updated successfully.";
                // Redirect to Index if deletion is successful
                return RedirectToAction(nameof(Index));
            }

            // If some other error occured
            // Get the Bad Request response
            DataResponse<Object>? responseContent = await response.Content.ReadFromJsonAsync<DataResponse<Object>>(
                new JsonSerializerOptions(JsonSerializerDefaults.Web));
            TempData["Errors"] = new string[] { responseContent?.Message ?? "Something went wrong while updating the category." };
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            // Attempt to delete the category
            HttpResponseMessage response = await _categoryService.DeleteAsync(id);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Category deleted successfully.";
                // Redirect to Index if deletion is successful
                return RedirectToAction(nameof(Index));
            }

            // If a bad request is returned, specifically if the category has tasks
            if ((int)response.StatusCode == 499)
            {
                // Get the Bad Request response
                DataResponse<List<ToDoTaskDisplayDto>>? responseContent = await response.Content.ReadFromJsonAsync<DataResponse<List<ToDoTaskDisplayDto>>>(
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));

                // If the response content is based on tasks depending on the category
                if (responseContent != null)
                {
                    List<string> errors = new List<string>();
                    if (responseContent.Data != null)
                    {
                        foreach (var task in responseContent.Data)
                        {
                            errors.Add($"Delete unsuccessful because the task ({task.Title}) is dependent on the category.");
                        }
                    }

                    TempData["Errors"] = errors.ToArray();
                    return RedirectToAction(nameof(Index));
                }

                // Add a generic error message if the response content is null
                TempData["Errors"] = new string[] { responseContent?.Message ?? $"Something went wrong while deleting the category with ID of {id}." };
                return RedirectToAction(nameof(Index));
            }

            // Return Index view with the list of categories and the error message
            TempData["Errors"] = new string[] { $"Something went wrong while deleting the category with ID of {id}." };
            return RedirectToAction(nameof(Index));
        }
    }
}
