using Microsoft.AspNetCore.Mvc;
using SimpleToDoAppWebMVC.Models;
using SharedDtoModels.TaskDtos;
using System.Diagnostics;
using System.Text.Json;

namespace SimpleToDoAppWebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client;

        public HomeController(
            ILogger<HomeController> logger,
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;

            // Log an error if HttpClientName is not configured
            var httpClientName = configuration["HttpClientName"];
            if (string.IsNullOrEmpty(httpClientName))
            {
                _logger.LogWarning("HttpClientName is not configured. Using default HttpClient.");
            }

            _client = httpClientFactory.CreateClient(httpClientName ?? "");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _client.GetFromJsonAsync<List<ToDoTaskDisplayDto>>(
                    "Tasks",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));

                return View(response);
            }
            catch (HttpRequestException ex)
            {
                if(ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return View(new List<ToDoTaskDisplayDto>());
                }
                _logger.LogError(ex, "An error occurred when calling the API.");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
