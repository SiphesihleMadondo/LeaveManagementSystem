using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDefaultRolesandUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "12db3bd5-272f-403b-8084-dd621c255ccd", null, "Employee", "EMPLOYEE" },
                    { "645d02c5-9557-4231-92d3-53d1b25df686", null, "Administrator", "ADMINISTRATOR" },
                    { "6602a374-627a-4023-8e05-b507c8b56fe8", null, "Supervisor", "SUPERVISOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c6db1b8f-4ef4-4710-9898-64789e612d7d", 0, "b9d19cc9-1f68-4825-895d-e959034d23c4", "admin@localhost.com", true, false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAIAAYagAAAAEMCK+prqwBx/yFHQ0ZfA135eqJ77PDUnsYuXfUcy6okXbPH+ZAFxP9atlJKaXiZ5dQ==", null, false, "3bb00221-41b1-452e-9ae7-0b818d091925", false, "admin@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "645d02c5-9557-4231-92d3-53d1b25df686", "c6db1b8f-4ef4-4710-9898-64789e612d7d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "12db3bd5-272f-403b-8084-dd621c255ccd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6602a374-627a-4023-8e05-b507c8b56fe8");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "645d02c5-9557-4231-92d3-53d1b25df686", "c6db1b8f-4ef4-4710-9898-64789e612d7d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "645d02c5-9557-4231-92d3-53d1b25df686");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c6db1b8f-4ef4-4710-9898-64789e612d7d");
        }
    }
}
