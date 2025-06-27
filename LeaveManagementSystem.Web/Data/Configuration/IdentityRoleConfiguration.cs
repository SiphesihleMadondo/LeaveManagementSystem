using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configuration
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        //even if you wanted default leave types you would do the same but use LeaveType type, you would do this with other tables 
        // this making sure that when creating a model you the above specified data.

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
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
        }
    }
}
