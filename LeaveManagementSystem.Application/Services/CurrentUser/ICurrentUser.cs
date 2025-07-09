namespace LeaveManagementSystem.Application.Services.CurrentUser
{
    public interface ICurrentUser
    {
        Task<ApplicationUser?> GetCurrentUser();
        Task<List<ApplicationUser>> GetEmployeesAsync();
        Task<ApplicationUser?> GetUserById(string userId);
        Task<IList<string>> GetUserRoles(string userId);
    }
}
