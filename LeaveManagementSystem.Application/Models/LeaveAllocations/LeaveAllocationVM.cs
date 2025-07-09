namespace LeaveManagementSystem.Application.Models.LeaveAllocations
{
    public class LeaveAllocationVM
    {
        public int Id { get; set; }

        [Display(Name = "Number of Days")]
        public decimal Days { get; set; }

        //ViewModels can only reference other viewModels
        [Display(Name = "Allocation Period")]
        public PeriodReadOnlyVM Period { get; set; } = new PeriodReadOnlyVM();

        public LeaveTypeReadOnlyVM LeaveType { get; set; } = new LeaveTypeReadOnlyVM();
    }
}
