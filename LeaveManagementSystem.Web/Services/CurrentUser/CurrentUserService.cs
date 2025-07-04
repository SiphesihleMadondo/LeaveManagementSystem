using Microsoft.AspNetCore.Identity;

namespace LeaveManagementSystem.Web.Services.CurrentUser
{
    public class CurrentUserService(UserManager<ApplicationUser> _userManager, IHttpContextAccessor _httpContextAccessor) : ICurrentUser
    {
        // Method to get the current user from the HttpContext
        public async Task<ApplicationUser?> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor?.HttpContext?.User);
            return user;
        }

        //method to get user by Id
        public async Task<ApplicationUser?> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        //method to get userRoles
        public async Task<IList<string>> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<string>();
            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }

        //get employees from the user manager
        public async Task<List<ApplicationUser>> GetEmployeesAsync()
        {
            //get all the users that are employees.
            var users = await _userManager.GetUsersInRoleAsync(StaticRoles.Employee);
            return users.ToList();

        }
    }

}
