using HalcyonAttendance.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using HalcyonAttendance.ViewModel;

namespace HalcyonAttendance.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<EmployeeDetails> EmployeeDetails { get; set; }
        public DbSet<VisitorDetails> VisitorDetails { get; set; }
        public DbSet<AttendanceModel> AttendanceModels { get; set; }
        public DbSet<HalcyonAttendance.ViewModel.RoleViewModel> RoleViewModel { get; set; }

    }
}
