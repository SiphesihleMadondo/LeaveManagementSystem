namespace LeaveManagementSystem.Application.Models.LeaveRequests
{
    public class LeaveRequestReadOnlyVM
    {

        public int Id { get; set; }

        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }

        [Display(Name = "Number Of Days")]
        public decimal NumberOfDays { get; set; }

        [Display(Name = "Leave Type")]
        public string LeaveTypeName { get; set; } = string.Empty;

        //leave request status
        [Display(Name = "Status")]
        public LeaveRequestStatusEnum LeaveRequestStatus { get; set; }

        public string? EmployeeFirstName { get; internal set; }

        public string? EmployeeLastName { get; internal set; }
    }
}