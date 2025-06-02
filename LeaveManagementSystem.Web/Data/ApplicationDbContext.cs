using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> //inheritence from the IdentityDbContext , and relative to pplicationUser
    {

        //(Constructor) get the info (options) when the application starts up, so you can use it here, and also pass it to base clase (IdentityDbContext)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }


        //overriding the OnModelCreating
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityRole>().HasData(
                //adding all the roles we want , 3 in this case
                new IdentityRole 
                { 
                    Id = "12db3bd5-272f-403b-8084-dd621c255ccd",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE" // Capitalized version of the Name.

                }, 
                new IdentityRole 
                {
                    Id = "6602a374-627a-4023-8e05-b507c8b56fe8",
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR"
                },
                new IdentityRole 
                {
                    Id = "645d02c5-9557-4231-92d3-53d1b25df686",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                });

            //even if you wanted default leave types you would do the same but use LeaveType type, you would do this with other tables 
            // this making sure that when creating a model you the above specified data.


            // a variable using a function for hashing
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.Entity<ApplicationUser>().HasData(

                new ApplicationUser
                {
                    Id = "c6db1b8f-4ef4-4710-9898-64789e612d7d",
                    Email = "admin@localhost.com",
                    NormalizedEmail = "ADMIN@LOCALHOST.COM",
                    NormalizedUserName = "ADMIN@LOCALHOST.COM",
                    UserName = "admin@localhost.com",
                    PasswordHash = hasher.HashPassword(null, "P@ssword1"), //put a hashed password, you initialise a variable using afunction for hashing
                    EmailConfirmed = true, // make it true for the successful login to occur
                    FirstName = "Default",
                    LastName = "Admin",
                    DateOfBirth = new DateOnly(1994,11,21)

                }

            );

            // associate the above user with a role (Administrator)
            // many to many role, 1 user to many roles, many users can be assigned to one role --> done using --> ApplicationUserRole
            builder.Entity<IdentityUserRole<string>>().HasData(
                
                    new IdentityUserRole<string>
                    {
                        RoleId = "645d02c5-9557-4231-92d3-53d1b25df686", // administrator role Id above
                        UserId = "c6db1b8f-4ef4-4710-9898-64789e612d7d" // value of the identity use Id above
                    }
                );

        }

        //the datatype of LeaveType which represent a set/collection of records
        // It is recommended to make the entity/table named plural
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }
        public DbSet<Period> Periods { get; set; }
    }

}
