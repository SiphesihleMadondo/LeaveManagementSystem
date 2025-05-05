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

        public IActionResult Index() //here's the action
        {
            //this where you define business logic
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            //can be queries
            //calculations e.t.c.
            var model = new ErrorViewModel // we initialising and creating an object of ErrorViewModel and storing it in abject called model
            {
                //RequestId is a property in ErrorViewModel
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier // and asigning this value to the RequestId

            };
            return View(model); // then you can say return view with the data --> View(model). sending the object to the view.
        }
    }
}
