using LeaveManagementSystem.Application.Models.Timesheet;
using LeaveManagementSystem.Application.Services.CurrentUser;
using LeaveManagementSystem.Application.Services.Timesheet;
using LeaveManagementSystem.Common.Static;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize(Roles = $"{StaticRoles.Administrator},{StaticRoles.Employee}")]
    public class TimesheetController(ICurrentUser _currentUser, ITimesheetService _timesheetService) : Controller
    {


        [HttpGet]
        [Authorize(Roles = StaticRoles.Employee)]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken = default)
        {
            var timesheet = await _timesheetService.TimesheetDetails(id);
            if (timesheet == null)
            {
                return NotFound();
            }
            // Check if the current user is the owner of the timesheet
            bool isOwner = await _timesheetService.IsTimesheetOwnerAsync(id, cancellationToken);
            if (!isOwner)
            {
                return Forbid(); // or RedirectToAction("Index")
            }

            return View(timesheet);
        }



        [HttpGet]
        [Authorize(Roles = StaticRoles.Employee)]
        public async Task<IActionResult> Index()
        {
            var timesheets = await _timesheetService.EmployeeTimeSummary();
            // Get all timesheets for current user (current month)
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;



            return View(timesheets);
        }

        [HttpGet]
        [Authorize(Roles = StaticRoles.Employee)]
        public IActionResult Create()
        {
            var model = new EmployeeTimesheetVM
            {
                WeekStartDate = DateTime.Today.StartOfWeek(DayOfWeek.Monday),
                Entries = Enumerable.Range(0, 5).Select(i => new TimesheetEntryVM
                {
                    Date = DateTime.Today.StartOfWeek(DayOfWeek.Monday).AddDays(i)
                }).ToList()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = StaticRoles.Employee)]
        public async Task<IActionResult> Create(EmployeeTimesheetVM employeeTimesheet, string actionType)
        {
            var startOfWeek = employeeTimesheet.WeekStartDate.Date;
            try
            {
                await _timesheetService.CreateTimesheet(employeeTimesheet, actionType);
            }
            catch (Exception ex)
            {
                // Optional: log error
                ModelState.AddModelError(string.Empty, "An error occurred while creating the timesheet.");
                return View(employeeTimesheet);
            }

            TempData["SuccessMessage"] = (actionType == "Submit")
                ? "Timesheet submitted successfully."
                : "Timesheet saved successfully.";

            return RedirectToAction("Index", new { weekStart = startOfWeek });
        }

        // GET: Timesheet/Review/5
        [HttpGet]
        [Authorize(Roles = StaticRoles.Administrator)]
        public async Task<IActionResult> Review(int id, CancellationToken cancellationToken = default)
        {
            var timesheet = await _timesheetService.GetTimesheetForReviewAsync(id, cancellationToken);
            if (timesheet == null)
            {
                return NotFound();
            }

            bool isOwner = await _timesheetService.IsTimesheetOwnerAsync(id, cancellationToken);
            if (isOwner)
            {
                return Forbid(); // Prevent self-approval
            }

            return View(timesheet); // expects a ReviewTimesheetVM
        }

        // POST: Timesheet/Review/5
        [HttpPost]
        [Authorize(Roles = StaticRoles.Administrator)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(int timesheetId, string adminComment, bool approved, CancellationToken cancellationToken = default)
        {
            var timesheet = await _timesheetService.GetTimesheetByIdAsync(timesheetId, cancellationToken);
            if (timesheet == null)
                return NotFound();

            if (await _timesheetService.IsTimesheetOwnerAsync(timesheetId, cancellationToken))
                return Forbid();

            try
            {
                if (approved)
                {
                    await _timesheetService.ApproveTimesheetAsync(timesheetId, adminComment, cancellationToken);
                }
                else
                {
                    await _timesheetService.DeclineTimesheetAsync(timesheetId, adminComment, cancellationToken);
                }

                TempData["SuccessMessage"] = "Timesheet reviewed successfully.";
                return RedirectToAction("Approval");
            }
            catch
            {
                ModelState.AddModelError("", "An error occurred while reviewing the timesheet.");
                return View(await _timesheetService.GetTimesheetForReviewAsync(timesheetId, cancellationToken)); // ensure this returns ReviewTimesheetVM
            }
        }



        // GET: LeaveTypes/Edit/5
        [HttpGet]
        [Authorize(Roles = StaticRoles.Employee)]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken = default)
        {
            var timesheet = await _timesheetService.GetTimesheetForEditAsync(id, cancellationToken);
            if (timesheet == null)
            {
                return NotFound();
            }
            if (!await _timesheetService.IsTimesheetOwnerAsync(id, cancellationToken))
            {
                return NotFound();
            }

            // Check if the current user is the owner of the timesheet
            bool isOwner = await _timesheetService.IsTimesheetOwnerAsync(id, cancellationToken);
            if (!isOwner)
            {
                return Forbid(); // or RedirectToAction("Index")
            }
            return View(timesheet);
        }

        //edit action method to review a timesheet by employee
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = StaticRoles.Employee)]
        public async Task<IActionResult> Edit(int id, EmployeeTimesheetVM employeeTimesheet, string actionType, CancellationToken cancellationToken = default)
        {
            if (id != employeeTimesheet.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(employeeTimesheet);

            try
            {
                await _timesheetService.EditTimesheet(employeeTimesheet, actionType, cancellationToken);
                TempData["SuccessMessage"] = "Timesheet updated successfully.";
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _timesheetService.DuplicateTimesheetExistsAsync(id, employeeTimesheet, cancellationToken))
                {
                    return NotFound();
                }

                ModelState.AddModelError(string.Empty, "An error occurred while updating the timesheet.");
                return View(employeeTimesheet);
            }
        }
        // GET: LeaveTypes/Delete/5
        [HttpGet]
        [Authorize(Roles = StaticRoles.Employee)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var timesheet = await _timesheetService.TimesheetDetails(id);
            if (timesheet == null)
            {
                return NotFound();
            }
            // Check if the current user is the owner of the timesheet
            bool isOwner = await _timesheetService.IsTimesheetOwnerAsync(id, cancellationToken);
            if (!isOwner)
            {
                return Forbid(); // or RedirectToAction("Index")
            }
            return View(timesheet);
        }

        //Delete action method to delete a timesheet by employee
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = StaticRoles.Employee)]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken = default)
        {
            var timesheet = await _timesheetService.TimesheetDetails(id);
            if (timesheet == null)
            {
                return NotFound();
            }
            // Check if the current user is the owner of the timesheet
            bool isOwner = await _timesheetService.IsTimesheetOwnerAsync(id, cancellationToken);
            if (!isOwner)
            {
                return Forbid(); // or RedirectToAction("Index")
            }
            try
            {
                await _timesheetService.DeleteTimesheet(id, cancellationToken);
                TempData["SuccessMessage"] = "Timesheet deleted successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the timesheet.");
                return View(timesheet);
            }
        }

        //GET dashboard for timesheet summary
        [HttpGet]
        [Authorize(Roles = StaticRoles.Administrator)]
        public async Task<IActionResult> Dashboard(CancellationToken cancellationToken = default)
        {
            var timesheetSummary = await _timesheetService.GetDashboardAsync(cancellationToken);
            if (timesheetSummary == null)
            {
                TempData["NoSummaryMessage"] = "No timesheets available.";
            }
            return View(timesheetSummary);
        }

        // GET: EmployeeActivity
        [HttpGet]
        [Authorize(Roles = StaticRoles.Administrator)]
        public async Task<IActionResult> EmployeeActivity(CancellationToken cancellationToken = default)
        {
            var employeeActivity = await _timesheetService.GetEmployeeActivitiesAsync(cancellationToken);
            if (employeeActivity == null || !employeeActivity.Any())
            {
                TempData["NoActivityMessage"] = "No employee activity available.";
            }
            return View(employeeActivity);
        }

        //Get approval GetApprovalQueueAsync
        [HttpGet]
        [Authorize(Roles = StaticRoles.Administrator)]
        public async Task<IActionResult> Approval(CancellationToken cancellationToken = default)
        {
            var approvalQueue = await _timesheetService.GetApprovalQueueAsync(cancellationToken);
            if (approvalQueue == null || !approvalQueue.Any())
            {
                TempData["NoApprovalMessage"] = "There are no timesheets currently pending approval.";
            }
            return View(approvalQueue);
        }


        // GET: Timesheet/Export
        [HttpGet]
        public async Task<IActionResult> Export(CancellationToken cancellationToken = default)
        {
            var timesheets = await _timesheetService.GetTimesheetsAsync(cancellationToken);
            if (timesheets == null || !timesheets.Any())
            {
                return NotFound("No timesheets available for export.");
            }
            // Logic to export timesheets (e.g., to CSV or Excel) goes here
            // For example, you can use a library like CsvHelper or EPPlus
            // Placeholder for export logic
            // var exportedFile = ExportToCsv(timesheets);
            // return File(exportedFile, "text/csv", "timesheets.csv");
            return View(timesheets); // Replace with actual export logic

        }
    }

}
