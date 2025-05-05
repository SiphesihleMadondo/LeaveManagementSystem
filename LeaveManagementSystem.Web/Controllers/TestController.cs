using LeaveManagementSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            var data = new TestViewModel //new object of type TestViewModel
            {
                Name = "Siphesihle Madondo",
                DateOfBirth = new DateTime(1994,11,21)
            };
            return View(data); // return and send the data to the view
        }
    }
}
