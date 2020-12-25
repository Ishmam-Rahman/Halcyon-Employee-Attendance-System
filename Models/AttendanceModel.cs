using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HalcyonAttendance.Models
{
    public class AttendanceModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:T}")]
        public DateTime ArrivalTime { get; set; }


        [DisplayFormat(DataFormatString = "{0:T}")]
        public DateTime LeavingTime { get; set; }

        [Required]
        [Display(Name = "Late Arrival/Early Leave")]
        public bool LateEarly { get; set; }
    }
}
