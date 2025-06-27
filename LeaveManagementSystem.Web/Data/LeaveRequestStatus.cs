namespace LeaveManagementSystem.Web.Data
{
    public class LeaveRequestStatus: BaseEntity
    {
        [StringLength(50)]
        public string Name { get; set; } = string.Empty; // a lookup table for the status of a leave request, e.g. Approved, Pending, Rejected

    }
}