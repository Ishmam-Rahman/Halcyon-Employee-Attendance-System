using Microsoft.EntityFrameworkCore.Migrations;

namespace HalcyonAttendance.Data.Migrations
{
    public partial class LateEarlyName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LateEarly",
                table: "AttendanceModels",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AttendanceModels",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LateEarly",
                table: "AttendanceModels");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AttendanceModels");
        }
    }
}
