using Microsoft.EntityFrameworkCore;
using SharedModels.TaskDtos;
using SimpleToDoAppAPI.Models;

namespace SimpleToDoAppAPI.Services
{
    public interface ITaskService
    {
        Task<List<ToDoTaskDisplayWithIdDto>> GetAllAsync();
    }

    public class TaskService : ITaskService, IDisposable
    {
        private readonly SimpleToDoAppDbContext _context;

        public TaskService(SimpleToDoAppDbContext context)
        {
            _context = context;
        }

        public void Dispose() => _context.Dispose();

        public async Task<List<ToDoTaskDisplayWithIdDto>> GetAllAsync()
        {
            List<ToDoTask> toDoTasks= await _context.ToDoTasks.Include(task => task.Category).ToListAsync<ToDoTask>();
            return toDoTasks.Select(task => task.ToDisplayWithIdDto()).ToList();
        }
    }
}
