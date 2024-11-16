using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Responses;
using SharedModels.TaskDtos;
using SimpleToDoAppAPI.Services;

namespace SimpleToDoAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        // Task service is used (This can only be available if registered in Program.cs)
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<ToDoTaskDisplayWithIdDto>? tasks = await _taskService.GetAllAsync();

            if (!tasks.Any())
            {
                return NotFound();
            }

            return Ok(new DataResponse<List<ToDoTaskDisplayWithIdDto>>
                {
                    Message = "Tasks retrieved successfully.",
                    Data = tasks
                });
        }
    }
}
