using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.CategoryDtos;
using SharedModels.TaskDtos;
using SimpleToDoAppAPI.Models;
using System.Linq;

namespace SimpleToDoAppAPI.Services
{
    public interface ICategoryService
    {
        Task<bool> AddAsync(CategoryDto category);

        Task<List<Category>> GetAllAsync();

        Task<Category?> GetByIdAsync(long id);

        Task<Category?> GetByNameAsync(string name);

        bool TryGetTasks(long id, out List<ToDoTaskDisplayDto> tasks);

        Task<bool> UpdateAsync(long id, CategoryDto newCategory);

        Task<bool> DeleteAsync(Category category);
    }

    public class CategoryService : ICategoryService, IDisposable
    {
        readonly SimpleToDoAppDbContext _context;

        public CategoryService(SimpleToDoAppDbContext context)
        {
            _context = context;
        }

        public void Dispose() => _context.Dispose();

        /// <summary>
        /// Saves a categoryDto object to Db, returns true if save is successful and false if not
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(CategoryDto category)
        {
            try
            {
                await _context.Categories.AddAsync(category.ToCategory());
                await _context.SaveChangesAsync();
                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            
        }

        /// <summary>
        /// Gets all categories as categoryDto
        /// </summary>
        /// <returns></returns>
        public async Task<List<Category>> GetAllAsync() =>
            await _context.Categories.ToListAsync<Category>();

        /// <summary>
        /// Gets a category as full category based on an ID, and returns null if none are found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Category?> GetByIdAsync(long id) =>
            await _context.Categories.FindAsync(id);

        /// <summary>
        /// Gets a category as full category based on a category name, and returns null if none are found
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<Category?> GetByNameAsync(string name)
            => await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);

        /// <summary>
        /// Try to get all tasks for a category, assuming a category exists
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public bool TryGetTasks(long id, out List<ToDoTaskDisplayDto> tasks)
        {
            Category? category = _context.Categories.Find(id);

            if (category == null) {
                tasks = new List<ToDoTaskDisplayDto>();
                return false;
            }

            List<ToDoTask> existingTasks = _context.ToDoTasks.Where(task => task.CategoryId == id).ToList<ToDoTask>();

            if(!existingTasks.Any()) {
                tasks = new List<ToDoTaskDisplayDto>();
                return false;
            }

            tasks = existingTasks.Select(task => task.ToDisplayDto()).ToList();
            return true;
        }


        /// <summary>
        /// Updates an already existing category, returns true if saved but false if the item being saved no longer exists
        /// </summary>
        /// <param name="existingCategory"></param>
        /// <param name="newCategory"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(long id, CategoryDto newCategory)
        {
            Category? category = await _context.Categories.FindAsync(id);

            // Return false if existing Category does not exists
            if (category == null)
            {
                return false;
            }

            // Update the contents of the category
            category.SetTo(newCategory);
            _context.Entry(category).State = EntityState.Modified;

            // Try to save the updated category
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // If the update fails because the existing category was deleted
                if (await GetByIdAsync(category.Id) == null)
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes a category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Category category)
        {
            try
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
        }
    }
}
