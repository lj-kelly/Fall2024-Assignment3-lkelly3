using Fall2024_Assignment3_lkelly3.Models;
using Fall2024_Assignment3_lkelly3.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Fall2024_Assignment3_lkelly3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly OpenAIhit _openAIhit;

        public HomeController(ILogger<HomeController> logger, OpenAIhit openAIhit)
        {
            _logger = logger;
            _openAIhit = openAIhit;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Movies()
        {
            return View();
        }
        public IActionResult Actors()
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
