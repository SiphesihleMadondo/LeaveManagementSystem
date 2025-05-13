using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext // inheritence from the IdentityDbContext , 
    {

        //(Constructor) get the info (options) when the application starts up, so you can use it here, and also pass it to base clase (IdentityDbContext)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        //the datatype of LeaveType which represent a set/collection of records
        // It is recommended to make the entity/table named plural
        public DbSet<LeaveType> LeaveTypes { get; set; }
    }

}
