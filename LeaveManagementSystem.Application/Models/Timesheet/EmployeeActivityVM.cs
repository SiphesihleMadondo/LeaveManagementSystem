using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Application.Models.Timesheet
{
    public class EmployeeActivityVM
    {
        public string? EmployeeName { get; set; }
        public bool SubmittedOnTime { get; set; }
        public int LateSubmissionsCount { get; set; }
        public int MissedSubmissionsCount { get; set; }
    }
}
