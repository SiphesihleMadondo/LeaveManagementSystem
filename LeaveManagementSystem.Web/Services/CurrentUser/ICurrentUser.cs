
namespace LeaveManagementSystem.Web.Services.CurrentUser
{
    public interface ICurrentUser
    {
        Task<ApplicationUser?>GetCurrentUser();
    }
}
