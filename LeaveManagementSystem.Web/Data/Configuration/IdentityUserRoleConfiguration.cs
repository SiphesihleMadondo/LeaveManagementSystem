using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configuration
{
    public class IdentityUserRoleConfiguration: IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            // associate the above user with a role (Administrator)
            // many to many role, 1 user to many roles, many users can be assigned to one role --> done using --> ApplicationUserRole
            builder.HasData(

                    new IdentityUserRole<string>
                    {
                        RoleId = "645d02c5-9557-4231-92d3-53d1b25df686", // administrator role Id above
                        UserId = "c6db1b8f-4ef4-4710-9898-64789e612d7d" // value of the identity use Id above
                    }
                );
        }
    }
}
