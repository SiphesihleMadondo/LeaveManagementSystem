using System.ComponentModel;

namespace LeaveManagementSystem.Web.Models.Periods
{
    public class PeriodEditVM: BasePeriodVM
    {
        public string Name { get; set; }


        [Required]
        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        [Required]
        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        [DateGreaterThan("StartDate", ErrorMessage = "End date must be after start date.")]
        public DateOnly EndDate { get; set; }

    }
}
