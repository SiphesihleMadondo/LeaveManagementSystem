using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedLeaveAllocationService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Days",
                table: "LeaveAllocations",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c6db1b8f-4ef4-4710-9898-64789e612d7d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e8ef6e7f-b802-4d28-bddf-bb734a0ffdaf", "AQAAAAIAAYagAAAAEBOeKPkxX3ZOrdjgmI7VeeTaeyBzmqdAg4751hgI9yn2q4OPYkYF2/oIJtpbaDM+MQ==", "3dfe93f7-4ea6-4a94-bcd9-f00aa207249e" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Days",
                table: "LeaveAllocations",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c6db1b8f-4ef4-4710-9898-64789e612d7d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78df24fd-8615-47ec-8059-3e16f062ba5f", "AQAAAAIAAYagAAAAEOe2sTI8XxH2RelG0ohu2wdlnbLiM/jiZd5oSho0K5DXdQwGr2FJFKHZVlSdNQ/F5w==", "1504267c-bfc1-4cf0-b639-5aadf4a00b81" });
        }
    }
}
