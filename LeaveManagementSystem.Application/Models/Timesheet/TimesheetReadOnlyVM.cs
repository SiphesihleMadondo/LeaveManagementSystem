

using static LeaveManagementSystem.Data.Timesheet;

namespace LeaveManagementSystem.Application.Models.Timesheet
{
    public class TimesheetReadOnlyVM
    {
       

        public int Id { get; set; }

        public string EmployeeFullName { get; set; } = string.Empty;

        public DateOnly WeekStartDate { get; set; }

        public TimesheetStatus Status { get; set; }

        public DateOnly? SubmittedAt { get; set; }

        public DateOnly? ReviewedAt { get; set; }

        public string? AdminComment { get; set; }

        public decimal TotalHours { get; set; }

        public decimal AverageHoursPerTimesheet { get; set; }

        public int TotalTimesheets { get; set; }

        public List<TimesheetEntryVM> Entries { get; set; } = new List<TimesheetEntryVM>();
        public Dictionary<TimesheetStatus, int> StatusCounts { get; set; } = new();
        public List<EmployeeTimesheetSummaryVM> RecentTimesheets { get; set; } = new();
        public int EntriesCount { get; set; }
    }
}
