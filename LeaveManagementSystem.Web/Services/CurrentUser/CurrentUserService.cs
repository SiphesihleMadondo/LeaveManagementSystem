using Microsoft.AspNetCore.Identity;

namespace LeaveManagementSystem.Web.Services.CurrentUser
{
    public class CurrentUserService(UserManager<ApplicationUser> _userManager, IHttpContextAccessor _httpContextAccessor):ICurreUser
    {
        public async Task<ApplicationUser?> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor?.HttpContext?.User);
            return user;
        }
    }
}
