using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Application.Models.Timesheet
{
    public class SubmissionHistoryVM
    {
        // Summary stats
        public int TotalTimesheetsThisWeek { get; set; }
        public int PendingApprovals { get; set; }
        public decimal AverageHoursPerEmployee { get; set; }
        public int MissingSubmissions { get; set; }

        // Approval queue
        public List<TimesheetApprovalQueueItemVM> ApprovalQueue { get; set; } = new();

        // Employee activity
        public List<EmployeeActivityVM> EmployeeActivities { get; set; } = new();

        // All submitted timesheets
        public List<TimesheetReadOnlyVM> AllSubmittedTimesheets { get; set; } = new();
    }
}
