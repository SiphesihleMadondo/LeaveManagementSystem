using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Data
{
    public class Timesheet
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; } // FK to Identity User
        public DateOnly WeekStartDate { get; set; } // e.g., Monday
        public List<TimesheetEntry> Entries { get; set; } = new List<TimesheetEntry>();
        //get employee first name and last name from the identity user table
        public ApplicationUser? Employee { get; set; } // Navigation property to ApplicationUser
        public TimesheetStatus Status { get; set; } = (TimesheetStatus)1;
        public string? AdminComment { get; set; }
        public DateOnly? SubmittedAt { get; set; }
        public DateOnly? ReviewedAt { get; set; }

        public enum TimesheetStatus
        {
            Draft = 0,
            Pending = 1,
            Approved = 2,
            Declined = 3
        }
    }

}
