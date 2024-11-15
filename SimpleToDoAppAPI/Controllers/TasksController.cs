using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleToDoAppAPI.Services;

namespace SimpleToDoAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskService.GetAllAsync();
            if (!tasks.Any())
            {
                return NotFound(new
                {
                    message = "There are no tasks available"
                });
            }

            return Ok(new
            {
                message = "Successfully retrieved tasks",
                tasks
            });
        }
    }
}
