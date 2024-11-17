using Microsoft.AspNetCore.Mvc;
using SimpleToDoAppWebMVC.Models;
using SharedModels.TaskDtos;
using System.Diagnostics;
using System.Text.Json;
using SimpleToDoAppWebMVC.Services;

namespace SimpleToDoAppWebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TaskService _taskService;

        public HomeController(ILogger<HomeController> logger, TaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        // The index page is where you'll view all the tasks
        public async Task<IActionResult> Index()
        {
            return View(await _taskService.GetAllAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
