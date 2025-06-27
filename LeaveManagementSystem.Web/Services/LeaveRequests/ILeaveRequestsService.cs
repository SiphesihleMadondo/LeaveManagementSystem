using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public interface ILeaveRequestsService
    {
        //should allow the creation of a leave request
        Task CreateLeaveRequest(LeaveRequestCreateVM leaveRequest);
        //employee can view his/her leave requests
        Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests();

        //Administrator / supervisor can view all leave requests
        Task<List<EmployeeLeaveRequestListVM>> GetAllLeaveRequests();

        //employee can cancel a leave request
        Task CancelLeaveRequest(int leaveRequestId);

        //Administrator / supervisor can review a leave request
        Task ReviewLeaveRequest(ReviewLeaveRequestVM reviewRequest);

        //makes sure the days don’t exceed the allocation.
        Task<bool> ValidateNumberOfDaysRequest(LeaveRequestCreateVM model);



    }
}