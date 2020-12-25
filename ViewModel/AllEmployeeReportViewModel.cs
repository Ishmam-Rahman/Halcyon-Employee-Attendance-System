using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HalcyonAttendance.ViewModel
{
    public class AllEmployeeReportViewModel
    {
        [Required]
        [Display(Name = "Image")]
        public string image { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required]
        [Display(Name = "Working Hour")]
        [DisplayFormat(DataFormatString = "{0:F1}")]
        public double WorkingHour { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Position")]
        public string position { get; set; }
    }
}
