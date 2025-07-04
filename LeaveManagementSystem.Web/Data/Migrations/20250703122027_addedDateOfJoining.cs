using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedDateOfJoining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c6db1b8f-4ef4-4710-9898-64789e612d7d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "50d86fe1-8899-4160-8e8d-32674db8d9fd", "AQAAAAIAAYagAAAAEFiZ4vPE0gUWkrpNCXz9bduBnajyf2SMD1uQwmXtLcfZ4kr0V5i6Vcz4gSjIt9ElaQ==", "85c2943b-2b24-433d-bf4d-b05b2726cd37" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c6db1b8f-4ef4-4710-9898-64789e612d7d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "212c3ef7-3689-4a6d-9245-9042e6730b0b", "AQAAAAIAAYagAAAAEIHBsBNrh9etxuxkUTl/UB7Kd/73A+SxyYkxOihotp8z3c8kqA266WCQot798tU/rw==", "615c1ba2-2aea-4f0e-bc15-0bf1e80395a0" });
        }
    }
}
