using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Data
{
    public class TimesheetEntry
    {
        public int Id { get; set; }
        public int TimesheetId { get; set; }
        public Timesheet? Timesheet { get; set; }

        public DateOnly Date { get; set; }
        [Precision(5, 2)]
        public decimal HoursWorked { get; set; }
        public string? TaskDescription { get; set; }
    }
}
