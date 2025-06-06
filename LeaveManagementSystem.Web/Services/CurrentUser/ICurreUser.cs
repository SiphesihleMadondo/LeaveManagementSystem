
namespace LeaveManagementSystem.Web.Services.CurrentUser
{
    public interface ICurreUser
    {
        Task<ApplicationUser?>GetCurrentUser();
    }
}
