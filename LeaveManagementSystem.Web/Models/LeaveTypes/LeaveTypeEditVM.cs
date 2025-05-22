using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    public class  LeaveTypeEditVM: BaseLeaveTypeVM
    {

        [Required]
        [Length(4, 150, ErrorMessage = "You have violeted the length requirements")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 90, ErrorMessage = "You have violeted the maximum number of days required, it must between 1 - 90")]
        [Display(Name = "Maximum Allocation of Days")]
        public decimal NumberOfDays { get; set; }
    }

}
