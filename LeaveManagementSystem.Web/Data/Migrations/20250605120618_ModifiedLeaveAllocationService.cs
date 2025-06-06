using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedLeaveAllocationService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c6db1b8f-4ef4-4710-9898-64789e612d7d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bd7b961b-3dc0-4fd2-beae-6f9d6596c5c6", "AQAAAAIAAYagAAAAEDoxeRtD4Gfuw+yxqdD3wPJNgIsvsIlVG4Wb5EmGsWqXCK5UEquipcas7G2cKKscbA==", "4ceb4319-3b42-4252-8907-c44ddc5c64ee" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c6db1b8f-4ef4-4710-9898-64789e612d7d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e8ef6e7f-b802-4d28-bddf-bb734a0ffdaf", "AQAAAAIAAYagAAAAEBOeKPkxX3ZOrdjgmI7VeeTaeyBzmqdAg4751hgI9yn2q4OPYkYF2/oIJtpbaDM+MQ==", "3dfe93f7-4ea6-4a94-bcd9-f00aa207249e" });
        }
    }
}
