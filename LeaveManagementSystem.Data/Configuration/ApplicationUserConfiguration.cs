using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Data.Configuration
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            // a variable using a function for hashing
            var hasher = new PasswordHasher<ApplicationUser>();
            builder.HasData(

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
                    DateOfBirth = new DateOnly(1994, 11, 21)

                }

            );
        }
    }
}
