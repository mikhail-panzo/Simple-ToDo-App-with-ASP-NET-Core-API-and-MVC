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

        public async Task<List<CategoryWithIdDto>?> GetAllAsync()
        {
            try
            {
                List<CategoryWithIdDto>? categories = (await _httpClient.GetFromJsonAsync<DataResponse<List<CategoryWithIdDto>>>(
                    "Categories",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)))?.Data;

                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting all categories: {Error}", ex);
            }

            return null;
        }

        public async Task<CategoryWithIdDto?> GetAsync(long id)
        {
            try
            {
                CategoryWithIdDto? category = (await _httpClient.GetFromJsonAsync<DataResponse<CategoryWithIdDto>>(
                    $"Categories/{id}",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web)))?.Data;

                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting the category: {Error}", ex);
            }

            return null;

        }

        public async Task<HttpResponseMessage> CreateAsync(CategoryDto categoryDto) =>
            await _httpClient.PostAsJsonAsync("Categories", categoryDto);

        public async Task<HttpResponseMessage> DeleteAsync(long id) =>
            await _httpClient.DeleteAsync($"Categories/{id}");

        public async Task<HttpResponseMessage> UpdateAsync(long id, CategoryDto categoryDto) =>
            await _httpClient.PutAsJsonAsync($"Categories/{id}", categoryDto);
    }
}
