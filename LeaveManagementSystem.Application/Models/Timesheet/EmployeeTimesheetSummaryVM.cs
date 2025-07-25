using static LeaveManagementSystem.Data.Timesheet;

namespace LeaveManagementSystem.Application.Models.Timesheet
{
    public class EmployeeTimesheetSummaryVM
    {
        public int Id { get; set; }
        public DateOnly WeekStartDate { get; set; }
        public TimesheetStatus Status { get; set; }
        public DateOnly? SubmittedAt { get; set; }
        public decimal WeeklyHours { get; set; }
        public decimal MonthlyHours { get; set; }
        public decimal AverageDaylyHours { get; set; }
        public int NoOfTimesheetSubmitted { get; set; }
        public DateOnly? LastSubmittedDate { get; set; }
        public decimal TotalHours { get;set ; }
    }
}