using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HalcyonAttendance.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Login Email")]
        [EmailAddress]
        public string LoginEmail { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}
