namespace LeaveManagementSystem.Web.Models.Periods
{
    public class PeriodReadOnlyVM: BasePeriodVM
    {
        public string Name { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateOnly EndDate { get; set; }
    }
}
