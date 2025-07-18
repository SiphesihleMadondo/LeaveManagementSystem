﻿namespace LeaveManagementSystem.Application.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task AllocateLeave(string employeeId, int selectedLeaveTypeId);
        Task<EmployeeAllocationVM> GetEmployeeAllocations(string? UserId);
        Task<LeaveAllocationEditVM> GetEmployeeAllocation(int AllocationId);
        Task<List<EmployeeListVM>> GetemployeesAsync();
        Task<int> GetLeaveTypeIdByNameAsync(string employee);
        Task UpdateLeaveAllocation(LeaveAllocationEditVM leaveAllocationEditVM);

        Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId);
        Task<bool> AllocationExists(string? UserId, int periodId, int leaveTypeId);
    }
}
