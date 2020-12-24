using Microsoft.EntityFrameworkCore.Migrations;

namespace HalcyonAttendance.Data.Migrations
{
    public partial class Id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AttendanceModels",
                table: "AttendanceModels");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AttendanceModels",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AttendanceModels",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttendanceModels",
                table: "AttendanceModels",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AttendanceModels",
                table: "AttendanceModels");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AttendanceModels");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AttendanceModels",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_AttendanceModels",
                table: "AttendanceModels",
                column: "Email");
        }
    }
}
