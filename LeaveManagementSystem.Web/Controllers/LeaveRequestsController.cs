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
        public async Task<IActionResult> Index(int allocationId)
        {
            var model = await _leaveRequestsService.GetEmployeeLeaveRequests();

            // Store allocationId in TempData to pass it to the View or later redirect
            TempData["AllocationId"] = allocationId;

            return View(model);
        }


        //employee can create a new leave request
        public async Task<IActionResult> Create(int? leaveTypeId)
        {
            var leaveTypes = await _leaveTypes.GetAll();

            //load the leave types for the dropdown in the view model
            var leaveTypesSelectList = leaveTypes.Select(lt => new SelectListItem
            {
                Value = lt.Id.ToString(),
                Text = lt.Name,
                Selected = leaveTypeId.HasValue && lt.Id == leaveTypeId.Value // Pre-select the leave type if provided

            }).ToList();

            //create the view model and pass the leave types to it
            var model = new LeaveRequestCreateVM
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now), // Default to today
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), // Default to tomorrow
                LeaveTypesList = leaveTypesSelectList
            };
            return View(model);
        }

        //post the new leave request and prevent CSRF attacks
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestCreateVM model)
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

            //reload the minumum data before rendering the view if nulls are present and re-assign and re-initalize the select list
            var leaveTypes = await _leaveTypes.GetAll();
            var leaveTypesSelectList = leaveTypes.Select(lt => new SelectListItem
            {
                Value = lt.Id.ToString(),
                Text = lt.Name
            }).ToList();

            model.LeaveTypesList = leaveTypesSelectList;
            return View(model);
        }

        //employee can cancel a leave request and we make sure to prevent CSRF attacks
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            // Logic to cancel the leave request
            await _leaveRequestsService.CancelLeaveRequest(id);
            return RedirectToAction("Index");
        }

        //Administrator / supervisor can view all leave requests
        [Authorize (Policy = "AdminSupervisorOnly")]
        public async Task<IActionResult> ListRequests()
        {
            // Logic to get all leave requests
            var model = await _leaveRequestsService.AdminGetAllLeaveRequests();
            return View(model);
        }

        //Administrator / supervisor can review a leave request
        [Authorize]
        public async Task<IActionResult> Review(int id)
        {
            // Logic to get the leave request by ID
            var model =  await _leaveRequestsService.GetLeaveRequestForReview(id);
            return View(model);
        }

        //post the review of a leave request and we make sure to prevent CSRF attacks
        [HttpPost]
        [ValidateAntiForgeryToken] 
        [Authorize]
        public async Task<IActionResult> ReviewLeaveRequest(int leaveRequestId, bool approved)
        {
            await _leaveRequestsService.ReviewLeaveRequest(leaveRequestId, approved);
            //redirect to the list of requests after review
            return RedirectToAction(nameof(ListRequests));
        }
    }
}
