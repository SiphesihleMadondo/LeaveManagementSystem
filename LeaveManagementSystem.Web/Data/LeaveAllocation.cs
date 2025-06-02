namespace LeaveManagementSystem.Web.Data
{
    public class LeaveAllocation: BaseEntity
    {

        //Link to the LeaveType Table.
        public LeaveType? LeaveType { get; set; } //Navigation Property, always make it NULLABLE

        // reference to the Leave Table you, by doing this you coupling it with the navigation property, it automatically know the relationship.
        public int LeaveTypeId { get; set; }

        //link to ApplicationUser Table
        public ApplicationUser? Employee { get; set; } //Navigation

        public string EmployeeId { get; set; } = nameof(Employee);

        //Link to the Period Table
        public Period? Period { get; set; } //Navigation

        public int PeriodId { get; set; }

        public int Days { get; set; }
    }
}
