using Microsoft.AspNetCore.Mvc;

namespace SimpleToDoAppWebMVC.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
