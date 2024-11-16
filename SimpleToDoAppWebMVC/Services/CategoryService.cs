using SharedModels.CategoryDtos;
using SharedModels.Responses;
using System.Net.Http;
using System.Text.Json;

namespace SimpleToDoAppWebMVC.Services
{
    public class CategoryService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(HttpClient httpClient, ILogger<CategoryService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public void Dispose() => _httpClient?.Dispose();

        public async Task<List<CategoryWithIdDto>> GetAllAsync()
        {
            try
            {
                List<CategoryWithIdDto>? categories = (await _httpClient.GetFromJsonAsync<DataResponse<List<CategoryWithIdDto>>>(
                    "Categories",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)))?.Data;

                return categories ?? new List<CategoryWithIdDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting all categories: {Error}", ex);
            }

            return new List<CategoryWithIdDto>();
        }

    }
}
