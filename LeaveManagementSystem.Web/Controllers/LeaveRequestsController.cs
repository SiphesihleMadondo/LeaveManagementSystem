using LeaveManagementSystem.Web.Models.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveRequestsController(ILeaveTypesService _leaveTypes, ILeaveRequestsService _leaveRequestsService) : Controller
    {

        //employee can view his/her leave requests
        public async Task<IActionResult> Index()
        {
            var model = await _leaveRequestsService.GetEmployeeLeaveRequests();
            return View(model);
        }

        //employee can create a new leave request
        public async Task<IActionResult> Create()
        {
            var leaveTypes = await _leaveTypes.GetAll();
            //load the leave types for the dropdown in the view model
            var leaveTypesSelectList = leaveTypes.Select(lt => new SelectListItem
            {
                Value = lt.Id.ToString(),
                Text = lt.Name
            }).ToList();

            //create the view model and pass the leave types to it
            var model = new LeaveRequestCreateVM
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now), // Default to today
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), // Default to tomorrow
                LeaveTypesList = leaveTypesSelectList // Assuming you have a property in your ViewModel to hold the leave types
            };
            return View(model);
        }

        //post the new leave request
        [HttpPost]
        [ValidateAntiForgeryToken] // To prevent CSRF attacks
        public async Task<IActionResult> Create(LeaveRequestCreateVM model /*will use ViewModel here*/)
        {
            //make sure the number of days does not exceed the number of days in the leave type allocation
            if (await _leaveRequestsService.ValidateNumberOfDaysRequest(model))
            {
                ModelState.AddModelError(string.Empty, "The number of days requested is invalid.");
                ModelState.AddModelError(nameof(model.EndDate), "The number of days requested exceeds your leave allocation for this leave type.");
            } 

            if (ModelState.IsValid)
            {
                // Logic to save the leave request
               await  _leaveRequestsService.CreateLeaveRequest(model);
                //redirect to the index page after successful creation
               return RedirectToAction("Index");
            }

            //reload the minumum data before rendering the view if nulls are present
            //which is the select list since it isn't bound to the form.
            //re-assign and re-initalize the select list
            var leaveTypes = await _leaveTypes.GetAll();
            var leaveTypesSelectList = leaveTypes.Select(lt => new SelectListItem
            {
                Value = lt.Id.ToString(),
                Text = lt.Name
            }).ToList();

            model.LeaveTypesList = leaveTypesSelectList;
            return View(model);
        }

        //employee can cancel a leave request
        [HttpPost]
        [ValidateAntiForgeryToken] // To prevent CSRF attacks
        public async Task<IActionResult> Cancel(int id)
        {
            // Logic to cancel the leave request
            await _leaveRequestsService.CancelLeaveRequest(id);
            return RedirectToAction("Index");
        }

        //Administrator / supervisor can view all leave requests
        [Authorize]
        public async Task<IActionResult> ListRequest()
        {
            // Logic to get all leave requests
            return View();
        }

        //Administrator / supervisor can review a leave request
        [Authorize]
        public async Task<IActionResult> Review(int leaveRequestId)
        {
            // Logic to get the leave request by ID
            return View();
        }

        //post the review of a leave request
        [HttpPost]
        [ValidateAntiForgeryToken] // To prevent CSRF attacks
        [Authorize]
        public async Task<IActionResult> Review(LeaveRequest leaveRequest /*will use ViewModel here*/)
        {
            if (ModelState.IsValid)
            {
                // Logic to update the leave request status
                return RedirectToAction("ListRequest");
            }
            return View(leaveRequest);
        }
    }
}
