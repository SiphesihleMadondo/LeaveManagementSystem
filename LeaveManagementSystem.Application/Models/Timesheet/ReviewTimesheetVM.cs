using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LeaveManagementSystem.Data.Timesheet;

namespace LeaveManagementSystem.Application.Models.Timesheet
{
    public class ReviewTimesheetVM
    {
        public int TimesheetId { get; set; }
        public DateOnly WeekStartDate { get; set; }
        public DateOnly? SubmittedAt { get; set; }
        public TimesheetStatus Status { get; set; }

        // Employee Information
        public string EmployeeFirstName { get; set; } = string.Empty; 
        public string EmployeeLastName { get; set; } = string.Empty; 

        // Entries
        public List<TimesheetEntryVM> Entries { get; set; } = new();
        public string? AdminComment { get;  set; }
        public string? EmployeeEmail { get; set; }
    }
}
