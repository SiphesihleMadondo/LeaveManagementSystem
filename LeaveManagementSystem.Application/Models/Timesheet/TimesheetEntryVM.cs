using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Application.Models.Timesheet
{
    public  class TimesheetEntryVM
    {
        [Required]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please enter the number of hours worked.")]
        [Range(0, 24, ErrorMessage = "Hours worked must be between 0 and 24.")]
        public decimal HoursWorked { get; set; }

        [Required(ErrorMessage = "Please describe the task.")]
        [StringLength(500, ErrorMessage = "Task description is too long.")]
        public string TaskDescription { get; set; } = string.Empty;
    }
}
