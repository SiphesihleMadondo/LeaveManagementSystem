



namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task AllocateLeave(string employeeId, int selectedLeaveTypeId);
        Task<EmployeeAllocationVM> GetEmployeeAllocations(string? UserId);
        Task<LeaveAllocationEditVM> GetEmployeeAllocation(int AllocationId);
        Task<List<EmployeeListVM>> GetemployeesAsync();
        Task<int> GetLeaveTypeIdByNameAsync(string employee);
        Task UpdateLeaveAllocation(LeaveAllocationEditVM leaveAllocationEditVM);
    }
}
