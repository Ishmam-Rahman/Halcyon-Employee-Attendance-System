using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HalcyonAttendance.Models
{
    public class VisitorDetails
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Time")]
        public DateTime dateTime { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Contact")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Type")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Proffesion")]
        public string Profession { get; set; }
    }
}
