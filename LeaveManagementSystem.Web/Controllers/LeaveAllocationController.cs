using LeaveManagementSystem.Web.Services.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService) : Controller
    {

        

        public async Task<IActionResult> Details()
        {
           var employeeAllocationVM = await _leaveAllocationsService.GetEmployeeAllocations();
           return View(employeeAllocationVM);
        }
    }
}
