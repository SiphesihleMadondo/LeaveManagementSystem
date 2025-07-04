using LeaveManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LeaveManagementSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(){ 
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //initialising and creating an object of ErrorViewModel and storing it in abject called model
            var model = new ErrorViewModel 
            {
                //asigning the value to the RequestId
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 

            };

            return View(model); 
        }
    }
}
