using SharedModels.TaskDtos;
using SharedModels.Responses;
using System.Text.Json;

namespace SimpleToDoAppWebMVC.Services
{
    public class TaskService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CategoryService> _logger;

        public TaskService(HttpClient httpClient, ILogger<CategoryService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public void Dispose() => _httpClient?.Dispose();

        public async Task<List<ToDoTaskDisplayWithIdDto>> GetAllAsync()
        {
            try
            {
                List<ToDoTaskDisplayWithIdDto>? tasks = (await _httpClient.GetFromJsonAsync<DataResponse<List<ToDoTaskDisplayWithIdDto>>>(
                    "Tasks",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)))?.Data;

                return tasks ?? new List<ToDoTaskDisplayWithIdDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting all tasks: {Error}", ex);
            }

            return new List<ToDoTaskDisplayWithIdDto>();
        }
    }
}
