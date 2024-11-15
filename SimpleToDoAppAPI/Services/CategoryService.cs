using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedDtoModels.CategoryDtos;
using SimpleToDoAppAPI.Models;
using System.Linq;

namespace SimpleToDoAppAPI.Services
{
    public interface ICategoryService
    {
        Task AddAsync(CategoryDto category);

        Task<List<CategoryDto>> GetAllAsync();
    }

    public class CategoryService : ICategoryService
    {
        readonly SimpleToDoAppDbContext _context;

        public CategoryService(SimpleToDoAppDbContext context)
        {
            _context = context;
        }

        // Saves a categoryDto object to Db
        public async Task AddAsync(CategoryDto category)
        {
            await _context.Categories.AddAsync(category.ToCategory());
            await _context.SaveChangesAsync();
        }

        // Gets all categories as categoryDto
        public async Task<List<CategoryDto>> GetAllAsync() =>
            (await _context.Categories.ToListAsync<Category>()).ToDto();
    }
}
