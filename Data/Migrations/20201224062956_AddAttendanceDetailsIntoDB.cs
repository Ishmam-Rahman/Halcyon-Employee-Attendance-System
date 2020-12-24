using Microsoft.EntityFrameworkCore.Migrations;

namespace HalcyonAttendance.Data.Migrations
{
    public partial class AddAttendanceDetailsIntoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LoginStatus",
                table: "EmployeeDetails",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoginStatus",
                table: "EmployeeDetails");
        }
    }
}
