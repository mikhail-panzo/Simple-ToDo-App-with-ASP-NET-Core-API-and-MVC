using Microsoft.EntityFrameworkCore;
using SharedDtoModels.TaskDtos;
using SimpleToDoAppAPI.Models;

namespace SimpleToDoAppAPI.Services
{
    public interface ITaskService
    {
        Task<List<ToDoTaskDisplayDto>> GetAllAsync();
    }

    public class TaskService : ITaskService
    {
        private readonly SimpleToDoAppDbContext _context;

        public TaskService(SimpleToDoAppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ToDoTaskDisplayDto>> GetAllAsync()
        {
            List<ToDoTask> toDoTasks= await _context.ToDoTasks.Include(task => task.Category).ToListAsync<ToDoTask>();
            return toDoTasks.Select(task => task.ToDisplayDto()).ToList();
        }
    }
}
