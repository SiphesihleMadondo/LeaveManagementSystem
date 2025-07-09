using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c6db1b8f-4ef4-4710-9898-64789e612d7d",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "FirstName", "LastName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "45c96ff9-fa75-44f8-8714-28d872bad57d", new DateOnly(1994, 11, 21), "Default", "Admin", "AQAAAAIAAYagAAAAEHG9p+IEEGOlBAMB7v9VVAA1DvSc9nJbzD0zGL+d/TGo6VvTkVDeNP2iZrJV9RqwLA==", "b3b49abf-0a4a-4782-8bfb-2f12ed3e2226" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c6db1b8f-4ef4-4710-9898-64789e612d7d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b9d19cc9-1f68-4825-895d-e959034d23c4", "AQAAAAIAAYagAAAAEMCK+prqwBx/yFHQ0ZfA135eqJ77PDUnsYuXfUcy6okXbPH+ZAFxP9atlJKaXiZ5dQ==", "3bb00221-41b1-452e-9ae7-0b818d091925" });
        }
    }
}
