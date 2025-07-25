using LeaveManagementSystem.Application.Models.Timesheet;

namespace LeaveManagementSystem.Application.Services.Timesheet
{
    public interface ITimesheetService
    {
        Task ApproveTimesheetAsync(int timesheetId, string adminComment, CancellationToken cancellationToken = default);
        Task CreateTimesheet(EmployeeTimesheetVM? employeeTimesheet, string actionType);
        Task DeclineTimesheetAsync(int timesheetId, string adminComment, CancellationToken cancellationToken = default);
        Task DeleteTimesheet(int id, CancellationToken cancellationToken = default);
        Task<bool> DuplicateTimesheetExistsAsync(int currentTimesheetId, EmployeeTimesheetVM? employeeTimesheet, CancellationToken cancellationToken);
        Task EditTimesheet(EmployeeTimesheetVM? employeeTimesheet, string actionType, CancellationToken cancellationToken = default);
        Task<TimesheetReadOnlyVM> EmployeeTimeSummary(CancellationToken cancellationToken = default);
        Task<List<TimesheetApprovalQueueItemVM>> GetApprovalQueueAsync(CancellationToken cancellationToken = default);
        Task<AdminTimesheetDashboardVM> GetDashboardAsync(CancellationToken cancellationToken = default);
        Task<List<EmployeeActivityVM>> GetEmployeeActivitiesAsync(CancellationToken cancellationToken = default);
        Task<Data.Timesheet> GetTimesheetByIdAsync(int timesheetId, CancellationToken cancellationToken);
        Task<EmployeeTimesheetVM?> GetTimesheetForEditAsync(int id, CancellationToken cancellationToken = default);
        Task<ReviewTimesheetVM?> GetTimesheetForReviewAsync(int id, CancellationToken cancellationToken);
        Task<List<EmployeeTimesheetSummaryVM>> GetTimesheetsAsync(CancellationToken cancellationToken = default);
        Task<bool> IsTimesheetOwnerAsync(int timesheetId, CancellationToken cancellationToken = default);
        Task<TimesheetReadOnlyVM> TimesheetDetails(int id);
    }
}