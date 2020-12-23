using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HalcyonAttendance.ViewModel
{
    public class EmployeeViewModel
    {
        [Display(Name = "Name")]
        public string EmpName { get; set; }


        [Required]
        [Display(Name = "Email")]
        public string EmpEmail { get; set; }


        [Required]
        [Display(Name = "Password")]
        public string EmpPassword { get; set; }


        [Required]
        [Display(Name = "Phone")]
        public string EmpPhone { get; set; }


        [Required]
        [Display(Name = "Image")]
        public IFormFile EmpImage { get; set; }


        [Required]
        [Display(Name = "Position")]
        public string EmpPosition { get; set; }

        [Required]
        [Display(Name = "Salary")]
        public int EmpSalary { get; set; }
    }
}
