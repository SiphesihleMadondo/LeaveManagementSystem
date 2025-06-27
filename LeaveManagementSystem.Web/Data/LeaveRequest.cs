namespace LeaveManagementSystem.Web.Data
{
    public class LeaveRequest: BaseEntity
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
      
        public LeaveType? LeaveType { get; set; } //Navigation Property, always make it NULLABLE
        public int LeaveTypeId { get; set; }

        public LeaveRequestStatus? RequestStatus { get; set; }
        public int RequestStatusId { get; set; } // link to the LeaveRequestStatus table

        public ApplicationUser? Employee { get; set; }
        public string EmployeeId { get; set; } = default!; // link to the ApplicationUser table

        public ApplicationUser? Reviewer { get; set; }
        public string? ReviewerId { get; set; } // link to the ApplicationUser table, make nullable as it may not be assigned in some cases

        public string? RequestComment { get; set; }
        
        
    }
}
