using AutoMapper;
using LeaveManagementSystem.Application.Models.Timesheet;
using LeaveManagementSystem.Application.Services.CurrentUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LeaveManagementSystem.Data.Timesheet;

namespace LeaveManagementSystem.Application.Services.Timesheet
{
    public class TimesheetService(ICurrentUser _currentUser, ApplicationDbContext _dbContext, IMapper _mapper) : ITimesheetService
    {

        //

        public async Task<TimesheetReadOnlyVM> EmployeeTimeSummary(CancellationToken cancellationToken = default)
        {
            var user = await _currentUser.GetCurrentUser();
         
            // Get all timesheets for current user (current month)
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;

            var timesheets = await _dbContext.Timesheets
                .Include(t => t.Entries)
                .Where(t => t.EmployeeId == user.Id &&
                            t.WeekStartDate.Month == currentMonth &&
                            t.WeekStartDate.Year == currentYear)
                .OrderByDescending(t => t.WeekStartDate)
                .ToListAsync(cancellationToken);

            var statusCounts = timesheets
                .GroupBy(t => t.Status)
                .ToDictionary(g => g.Key, g => g.Count());

            var averageHours = timesheets.Count > 0
                ? Math.Round(timesheets.Sum(t => t.Entries.Sum(e => e.HoursWorked)) / timesheets.Count, 2)
                : 0;

            var totalHours = timesheets.Sum(t => t.Entries.Sum(e => e.HoursWorked));

            var summaryVM = new TimesheetReadOnlyVM
            {
                StatusCounts = statusCounts,
                AverageHoursPerTimesheet = averageHours,
                TotalHours = totalHours,
                TotalTimesheets = timesheets.Count,
                RecentTimesheets = timesheets.Select(t => new EmployeeTimesheetSummaryVM
                {
                    Id = t.Id,
                    WeekStartDate = t.WeekStartDate,
                    Status = t.Status,
                    SubmittedAt = t.SubmittedAt,
                    TotalHours = t.Entries.Sum(e => e.HoursWorked)
                }).ToList()
            };

            return summaryVM;
        }



        public async Task<TimesheetReadOnlyVM> TimesheetDetails(int id)
        {
            var timesheet = await _dbContext.Timesheets
                .Include(t => t.Entries)
                .Include(t => t.Employee) // assuming you want the employee name
                .FirstOrDefaultAsync(t => t.Id == id);

            var vm = new TimesheetReadOnlyVM
            {
                Id = timesheet.Id,
                EmployeeFullName = $"{timesheet.Employee.FirstName} {timesheet.Employee.LastName}",
                WeekStartDate = timesheet.WeekStartDate,
                Status = timesheet.Status,
                SubmittedAt = timesheet.SubmittedAt,
                ReviewedAt = timesheet.ReviewedAt,
                AdminComment = timesheet.AdminComment,
                Entries = timesheet.Entries.Select(e => new TimesheetEntryVM
                {
                    Date = e.Date.ToDateTime(TimeOnly.MinValue),
                    HoursWorked = e.HoursWorked,
                    TaskDescription = e.TaskDescription
                }).ToList()
            };

            vm = _mapper.Map<TimesheetReadOnlyVM>(timesheet);
            return vm;
        }

        // Check if the current user is the owner of the timesheet
        public async Task<bool> IsTimesheetOwnerAsync(int timesheetId, CancellationToken cancellationToken = default)
        {
            var user = await _currentUser.GetCurrentUser();
            if (user == null)
                return false;
            return await _dbContext.Timesheets.AnyAsync(t => t.Id == timesheetId && t.EmployeeId == user.Id, cancellationToken);
        }

        public async Task<List<EmployeeTimesheetSummaryVM>> GetTimesheetsAsync(CancellationToken cancellationToken = default)
        {
            var user = await _currentUser.GetCurrentUser();

            if (user == null)
                return [];

            var timesheets = await _dbContext.Timesheets
                .Where(t => t.EmployeeId == user.Id)
                .OrderByDescending(t => t.WeekStartDate)
                .ToListAsync(cancellationToken);

            return timesheets.Select(t => new EmployeeTimesheetSummaryVM
            {
                Id = t.Id,
                WeekStartDate = t.WeekStartDate,
                Status = t.Status,
                SubmittedAt = t.SubmittedAt
            }).ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task CreateTimesheet(EmployeeTimesheetVM? employeeTimesheet, string actionType)
        {
            var user = await _currentUser.GetCurrentUser();

            var existingTimesheet = _mapper.Map<LeaveManagementSystem.Data.Timesheet>(employeeTimesheet);

            //check if the timesheet already exists for the user
            existingTimesheet = await _dbContext.Timesheets.
            Include(t => t.Entries).
            FirstOrDefaultAsync(t => t.EmployeeId == user.Id && t.WeekStartDate == DateOnly.FromDateTime(employeeTimesheet.WeekStartDate.Date));

            if (existingTimesheet != null)
            {

                //Remove previous entries
                _dbContext.TimesheetEntries.RemoveRange(existingTimesheet.Entries);

                existingTimesheet.Entries = employeeTimesheet.Entries.Select(e => new TimesheetEntry
                {
                    Date = DateOnly.FromDateTime(e.Date),
                    HoursWorked = e.HoursWorked,
                    TaskDescription = e.TaskDescription
                }).ToList();

                existingTimesheet.SubmittedAt = (actionType == "Submit") ? DateOnly.FromDateTime(DateTime.UtcNow) : null;
                existingTimesheet.Status = (TimesheetStatus)((actionType == "Submit") ?
                    (int)TimesheetStatus.Pending
                    : (int)TimesheetStatus.Draft);
            }
            else
            {
                //create a new timesheet
                var newTimesheet = new LeaveManagementSystem.Data.Timesheet
                {
                    EmployeeId = user.Id,
                    WeekStartDate = DateOnly.FromDateTime(employeeTimesheet.WeekStartDate.Date),
                    Entries = employeeTimesheet.Entries.Select(e => new TimesheetEntry
                    {
                        Date = DateOnly.FromDateTime(e.Date),
                        HoursWorked = e.HoursWorked,
                        TaskDescription = e.TaskDescription
                    }).ToList(),
                    SubmittedAt = (actionType == "Submit") ? DateOnly.FromDateTime(DateTime.UtcNow) : null,
                    Status = (TimesheetStatus)((actionType == "Submit") ?
                        (int)TimesheetStatus.Pending
                        : (int)TimesheetStatus.Draft)
                };
                await _dbContext.Timesheets.AddAsync(newTimesheet);
            }

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

        }

        // edit
        public async Task<EmployeeTimesheetVM?> GetTimesheetForEditAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _currentUser.GetCurrentUser();
            if (user == null)
                return null;
            var timesheet = await _dbContext.Timesheets
                .Include(t => t.Entries)
                .FirstOrDefaultAsync(t => t.Id == id && t.EmployeeId == user.Id, cancellationToken);
            if (timesheet == null)
                return null;
            var employeeTimesheet = new EmployeeTimesheetVM
            {
                Id = timesheet.Id,
                WeekStartDate = timesheet.WeekStartDate.ToDateTime(TimeOnly.MinValue),
                Entries = timesheet.Entries.Select(e => new TimesheetEntryVM
                {

                    Date = e.Date.ToDateTime(TimeOnly.MinValue),
                    HoursWorked = e.HoursWorked,
                    TaskDescription = e.TaskDescription
                }).ToList()
            };
            return employeeTimesheet;
        }


        //edit timesheet
        public async Task EditTimesheet(EmployeeTimesheetVM? employeeTimesheet, string actionType, CancellationToken cancellationToken = default)
        {
            var user = await _currentUser.GetCurrentUser();
            if (user == null || employeeTimesheet == null)
                return;

            var existingTimesheet = await _dbContext.Timesheets
                .Include(t => t.Entries)
                .FirstOrDefaultAsync(t => t.EmployeeId == user.Id && t.Id == employeeTimesheet.Id, cancellationToken);

            if (existingTimesheet != null)
            {
                // Replace old entries with new
                _dbContext.TimesheetEntries.RemoveRange(existingTimesheet.Entries);

                existingTimesheet.Entries = employeeTimesheet.Entries.Select(e => new TimesheetEntry
                {
                    Date = DateOnly.FromDateTime(e.Date),
                    HoursWorked = e.HoursWorked,
                    TaskDescription = e.TaskDescription
                }).ToList();

                if (actionType == "Submit")
                {
                    existingTimesheet.SubmittedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                    existingTimesheet.Status = TimesheetStatus.Pending;
                }
                else
                {
                    existingTimesheet.SubmittedAt = null;
                    existingTimesheet.Status = TimesheetStatus.Draft;
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        //delete timesheet
        public async Task DeleteTimesheet(int id, CancellationToken cancellationToken = default)
        {
            var user = await _currentUser.GetCurrentUser();
            if (user == null)
                return;
            var timesheet = await _dbContext.Timesheets
                .Include(t => t.Entries)
                .FirstOrDefaultAsync(t => t.Id == id && t.EmployeeId == user.Id, cancellationToken);
            if (timesheet != null)
            {
                _dbContext.TimesheetEntries.RemoveRange(timesheet.Entries);
                _dbContext.Timesheets.Remove(timesheet);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        //GetPendingTimesheetsForUserAsync
        public async Task<List<TimesheetReadOnlyVM>> GetPendingTimesheetsForUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            var timesheets = await _dbContext.Timesheets
                .Include(t => t.Employee)
                .Include(t => t.Entries)
                .Where(t => t.EmployeeId == userId && t.Status != TimesheetStatus.Approved)
                .OrderByDescending(t => t.WeekStartDate)
                .ToListAsync(cancellationToken);

            return timesheets.Select(t => new TimesheetReadOnlyVM
            {
                Id = t.Id,
                WeekStartDate = t.WeekStartDate,
                Status = t.Status,
                SubmittedAt = t.SubmittedAt,
                TotalHours = t.Entries.Sum(e => e.HoursWorked),
                EmployeeFullName = $"{t.Employee.FirstName} {t.Employee.LastName}"
            }).ToList();
        }


        public async Task<bool> DuplicateTimesheetExistsAsync(int currentTimesheetId, EmployeeTimesheetVM? employeeTimesheet, CancellationToken cancellationToken)
        {
            var user = await _currentUser.GetCurrentUser();
            if (user == null || employeeTimesheet == null)
                return false;

            var weekStartDate = DateOnly.FromDateTime(employeeTimesheet.WeekStartDate.Date);

            return await _dbContext.Timesheets.AnyAsync(t =>
                t.EmployeeId == user.Id &&
                t.WeekStartDate == DateOnly.FromDateTime(employeeTimesheet.WeekStartDate) &&
                t.Id != currentTimesheetId, // Exclude the one being edited
                cancellationToken);
        }

        // Admin Timesheet Dashboard
        public async Task<AdminTimesheetDashboardVM> GetDashboardAsync(CancellationToken cancellationToken = default)
        {
            //get current week
            var currentWeekStart = DateTime.UtcNow.StartOfWeek(DayOfWeek.Monday);

            //get all the timesheets for the current month
            var timesheets = await _dbContext.Timesheets
                .Include(t => t.Employee)
                .Include(t => t.Entries)
                .Where(t => t.SubmittedAt != null) // Optional: only include submitted timesheets
                .OrderByDescending(t => t.WeekStartDate)
                .ToListAsync(cancellationToken);

            // this week timesheets
            var thisWeekTimesheets = timesheets.
                Where(timesheet=> timesheet.WeekStartDate == DateOnly.FromDateTime(currentWeekStart)).ToList();

            // submitted timesheets
            var allSubmittedTimesheets = timesheets
            .Where(t => t.SubmittedAt != null)
            .OrderByDescending(t => t.WeekStartDate).Select(t => new TimesheetReadOnlyVM
            {
                Id = t.Id,
                EmployeeFullName = $"{t.Employee.FirstName} {t.Employee.LastName}",
                WeekStartDate = t.WeekStartDate,
                Status = t.Status,
                SubmittedAt = t.SubmittedAt,
                ReviewedAt = t.ReviewedAt,
                AdminComment = t.AdminComment,
                TotalHours = t.Entries.Sum(e => e.HoursWorked),
                EntriesCount = t.Entries.Count
            }).ToList();

            //dashboard view model
            var dashboard = new AdminTimesheetDashboardVM
            {
                TotalTimesheetsThisWeek = allSubmittedTimesheets.Count,
                PendingApprovals = allSubmittedTimesheets.Count(t => t.Status == TimesheetStatus.Pending),
                AverageHoursPerEmployee = (decimal)(double)(allSubmittedTimesheets.Count > 0
                    ? Math.Round(allSubmittedTimesheets.Average(t => t.TotalHours), 2)
                    : 0),
                MissingSubmissions = allSubmittedTimesheets.Count(t => t.Status == TimesheetStatus.Draft),
                AllSubmittedTimesheets = allSubmittedTimesheets
            };

            return dashboard;
        }

        //emplyee activity
        public async Task<List<EmployeeActivityVM>> GetEmployeeActivitiesAsync(CancellationToken cancellationToken = default)
        {
            var timesheets = await _dbContext.Timesheets
                .Include(t => t.Employee)
                .Where(t => t.EmployeeId != null)
                .OrderByDescending(t => t.WeekStartDate)
                .ToListAsync(cancellationToken);
            return timesheets.Select(t => new EmployeeActivityVM
            {
                EmployeeName = $"{t.Employee.FirstName} {t.Employee.LastName}",
                SubmittedOnTime = t.SubmittedAt.HasValue && t.SubmittedAt.Value <= DateOnly.FromDateTime(DateTime.UtcNow),
                LateSubmissionsCount = timesheets.Count(ts => ts.EmployeeId == t.EmployeeId && ts.SubmittedAt > ts.WeekStartDate.AddDays(6)), //week ends on Sunday
                MissedSubmissionsCount = timesheets.Count(ts => ts.EmployeeId == t.EmployeeId && ts.Status == TimesheetStatus.Draft),
            }).ToList();
        }

        //Approval queue for admin
        public async Task<List<TimesheetApprovalQueueItemVM>> GetApprovalQueueAsync(CancellationToken cancellationToken = default)
        {
            var timesheets = await _dbContext.Timesheets
                .Include(t => t.Employee)
                .Include(t => t.Entries)
                .Where(t => t.Status == TimesheetStatus.Pending)
                .OrderByDescending(t => t.WeekStartDate)
                .ToListAsync(cancellationToken);
            return timesheets.Select(t => new TimesheetApprovalQueueItemVM
            {
                Id = t.Id,
                EmployeeName = $"{t.Employee.FirstName} {t.Employee.LastName}",
                WeekStartDate = t.WeekStartDate,
                SubmittedAt = t.SubmittedAt,
                ReviewedAt = t.ReviewedAt,
                Status = t.Status,
                TotalHours = t.Entries.Sum(e => e.HoursWorked),
                AdminComment = t.AdminComment
            }).ToList();
        }


        //Admin approval of timesheets
        public async Task ApproveTimesheetAsync(int timesheetId, string adminComment, CancellationToken cancellationToken = default)
        {
            var timesheet = await _dbContext.Timesheets
                .Include(t => t.Entries)
                .FirstOrDefaultAsync(t => t.Id == timesheetId, cancellationToken);
            if (timesheet != null)
            {
                timesheet.Status = TimesheetStatus.Approved;
                timesheet.ReviewedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                timesheet.AdminComment = adminComment;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        //Admin decline of timesheets
        public async Task DeclineTimesheetAsync(int timesheetId, string adminComment, CancellationToken cancellationToken = default)
        {
            var timesheet = await _dbContext.Timesheets
                .Include(t => t.Entries)
                .FirstOrDefaultAsync(t => t.Id == timesheetId, cancellationToken);
            if (timesheet != null)
            {
                timesheet.Status = TimesheetStatus.Declined;
                timesheet.ReviewedAt = DateOnly.FromDateTime(DateTime.UtcNow);
                timesheet.AdminComment = adminComment;
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        //Review Timesheet/Review/1012
        public async Task<ReviewTimesheetVM?> GetTimesheetForReviewAsync(int id, CancellationToken cancellationToken)
        {
            var timesheet = await _dbContext.Timesheets
                 .Include(t => t.Employee)
                 .Include(t => t.Entries)
                 .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (timesheet == null)
                return null;

            return new ReviewTimesheetVM
            {
                TimesheetId = timesheet.Id,
                WeekStartDate = timesheet.WeekStartDate,
                SubmittedAt = timesheet.SubmittedAt,
                Status = timesheet.Status,
                EmployeeFirstName = timesheet.Employee.FirstName,
                EmployeeLastName = timesheet.Employee.LastName,
                EmployeeEmail = timesheet.Employee.Email,
                Entries = timesheet.Entries.Select(e => new TimesheetEntryVM
                {
                    Date = e.Date.ToDateTime(TimeOnly.MinValue),
                    HoursWorked = e.HoursWorked,
                    TaskDescription = e.TaskDescription
                }).ToList()
            };
        }

        public async Task<Data.Timesheet> GetTimesheetByIdAsync(int timesheetId, CancellationToken cancellationToken)
        {
              var timesheet = await _dbContext.Timesheets
                .Include(t => t.Entries)
                .FirstOrDefaultAsync(t => t.Id == timesheetId, cancellationToken);
            if (timesheet == null)
            {
                throw new KeyNotFoundException($"Timesheet with ID {timesheetId} not found.");
            }
            return timesheet;
        }


    }
}
