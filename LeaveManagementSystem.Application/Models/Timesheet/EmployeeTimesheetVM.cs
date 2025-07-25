using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Application.Models.Timesheet
{
    public  class EmployeeTimesheetVM
    {
        public int Id { get; set; }
        public DateTime WeekStartDate { get; set; }
        public List<TimesheetEntryVM> Entries { get; set; } = new();
        
    }
}
