using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class changedRequestCommentToacceptNulls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RequestComment",
                table: "LeaveRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c6db1b8f-4ef4-4710-9898-64789e612d7d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0552333e-a5c3-431f-9adb-06d0a9cf38ae", "AQAAAAIAAYagAAAAEGnFR9ves55B/8D2Um82JT7JhooFon24XPq8V2rfQJd6oddICmquIZsQANE2gXRHTw==", "38782fec-0a3c-472d-abef-90b5aee2c700" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RequestComment",
                table: "LeaveRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c6db1b8f-4ef4-4710-9898-64789e612d7d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1d8574d2-0ba8-4d0d-aff8-fb09b4c00245", "AQAAAAIAAYagAAAAEDkSzT9O+CSoob86HpOcMf5L4YNgvD7y+regkhAL8lbl2gBB7ypli/2FmdzrY9VbrA==", "a95278c5-c14f-46d2-a16a-7eec523bea29" });
        }
    }
}
