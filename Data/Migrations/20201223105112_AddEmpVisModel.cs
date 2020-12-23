using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HalcyonAttendance.Data.Migrations
{
    public partial class AddEmpVisModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpName = table.Column<string>(nullable: false),
                    EmpEmail = table.Column<string>(nullable: false),
                    EmpPassword = table.Column<string>(nullable: false),
                    EmpPhone = table.Column<string>(nullable: false),
                    EmpImage = table.Column<string>(nullable: false),
                    EmpPosition = table.Column<string>(nullable: false),
                    EmpSalary = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VisitorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateTime = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Profession = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorDetails", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeDetails");

            migrationBuilder.DropTable(
                name: "VisitorDetails");
        }
    }
}
