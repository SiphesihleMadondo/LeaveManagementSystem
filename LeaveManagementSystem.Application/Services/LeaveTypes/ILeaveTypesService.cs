namespace LeaveManagementSystem.Application.Services.LeaveTypes
{
    public interface ILeaveTypesService
    {
        Task<bool> CheckIfLeaveTypeExistAsync(string name);
        Task<bool> CheckIfLeaveTypeExistForEditAsync(LeaveTypeEditVM leaveTypeEditVM);
        Task Create(LeaveTypeCreateVM model);
        Task Edit(LeaveTypeEditVM model);
        Task<T?> Get<T>(int id) where T : class;
        Task<List<LeaveTypeReadOnlyVM>> GetAll();
        Task<bool> IsLeaveTypeValid(int leaveTypeId, decimal days);
        Task<bool> LeaveTypeExists(int id);
        Task Remove(int id);
    }
}