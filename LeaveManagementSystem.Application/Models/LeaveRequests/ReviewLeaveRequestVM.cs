namespace LeaveManagementSystem.Application.Models.LeaveRequests
{
    public class ReviewLeaveRequestVM : LeaveRequestReadOnlyVM
    {
        public EmployeeListVM Employee { get; set; } = new EmployeeListVM();
        [Display(Name = "Additional Information")]
        public string? RequestComments { get; set; }
    }
}