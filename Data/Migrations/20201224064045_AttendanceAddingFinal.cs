using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HalcyonAttendance.Data.Migrations
{
    public partial class AttendanceAddingFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendanceModels",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    ArrivalTime = table.Column<DateTime>(nullable: false),
                    LeavingTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceModels", x => x.Email);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceModels");
        }
    }
}
