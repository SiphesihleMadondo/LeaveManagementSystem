using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedEmployeeAllocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c6db1b8f-4ef4-4710-9898-64789e612d7d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bf835885-d21a-4c7d-9f43-e8a0af67ce16", "AQAAAAIAAYagAAAAEF2RswT6aCkHopzo3/UhZSyVJhGV9k/orUjBcn3BOAk5N0cs/5D7sFIXdcAOteQg8w==", "ca861461-1be2-4d80-8d85-4d2e828d0563" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c6db1b8f-4ef4-4710-9898-64789e612d7d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bd7b961b-3dc0-4fd2-beae-6f9d6596c5c6", "AQAAAAIAAYagAAAAEDoxeRtD4Gfuw+yxqdD3wPJNgIsvsIlVG4Wb5EmGsWqXCK5UEquipcas7G2cKKscbA==", "4ceb4319-3b42-4252-8907-c44ddc5c64ee" });
        }
    }
}
