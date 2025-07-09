using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Services.LeaveAllocations;
using LeaveManagementSystem.Application.Services.LeaveTypes;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService, ILeaveTypesService _leaveTypes) : Controller
    {


        [Authorize(Roles = StaticRoles.Administrator)]
        //allows the admin to view the list of employees and jump into each employee details.
        public async Task<IActionResult> Index()
        {
            var employees = await _leaveAllocationsService.GetemployeesAsync();
            return View(employees);
        }


        //allows the admin to view allocate leave
        [Authorize(Roles = StaticRoles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateLeave(string? employeeId, int? leaveTypeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId) || leaveTypeId == null)
            {
                return BadRequest("Employee ID or Leave Type ID is missing.");
            }

            await _leaveAllocationsService.AllocateLeave(employeeId, leaveTypeId.Value);

            return RedirectToAction(nameof(Details), new { UserId = employeeId });
        }

        [Authorize(Roles = StaticRoles.Administrator)]
        public async Task<IActionResult> EditAllocation(int? id)
        {
            if (id == null)
            {

                return NotFound();
            }

            var allocation = await _leaveAllocationsService.GetEmployeeAllocation(id.Value);

            if (allocation == null)
            {
                return BadRequest();
            }

            return View(allocation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAllocation(LeaveAllocationEditVM leaveAllocation)
        {
            if (!ModelState.IsValid)
            {
                //checking for property errors in ModelState
                foreach (var error in ModelState)
                {
                    foreach (var subError in error.Value.Errors)
                    {
                        Console.WriteLine($"Property: {error.Key}, Error: {subError.ErrorMessage}");
                    }
                }
                return View(leaveAllocation);
            }

            // check if the leave type is valid and if the number of days is valid
            if (await _leaveTypes.IsLeaveTypeValid(leaveAllocation.LeaveType.Id, leaveAllocation.Days))
            {
                ModelState.AddModelError("Days", "The allocation exceeds the valid number of days for this leave type.");
            }

            //if model state is valid, proceed to update the allocation
            if (ModelState.IsValid)
            {
                await _leaveAllocationsService.UpdateLeaveAllocation(leaveAllocation);
                return RedirectToAction(nameof(Details), new { UserId = leaveAllocation?.employee?.Id });
            }

            var days = leaveAllocation.Days;
            // If the model state is not valid, return the view with the current leave allocation data
            var allocationFromDb = await _leaveAllocationsService.GetEmployeeAllocation(leaveAllocation.Id);
            var daysFromDb = allocationFromDb.Days;
            return View(leaveAllocation);
        }

        // pass the id of the employee signed in, 
        //allowing the admin
        public async Task<IActionResult> Details(string? UserId)
        {
            var employeeAllocationVM = await _leaveAllocationsService.GetEmployeeAllocations(UserId);
            return View(employeeAllocationVM);
        }


    }
}
