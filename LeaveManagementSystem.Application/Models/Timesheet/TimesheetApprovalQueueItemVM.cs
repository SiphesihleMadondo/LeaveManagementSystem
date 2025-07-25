using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LeaveManagementSystem.Data.Timesheet;

namespace LeaveManagementSystem.Application.Models.Timesheet
{
    public  class TimesheetApprovalQueueItemVM
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; } // FK 
        public string EmployeeName { get; set; }
        public DateOnly WeekStartDate { get; set; }
        public TimesheetStatus Status { get; set; } 
        public DateOnly? SubmittedAt { get; set; }
        public DateOnly? ReviewedAt { get; set; }
        public string? AdminComment { get; set; }
        public decimal TotalHours { get; set; }
    }
}
