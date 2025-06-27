using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Models.LeaveRequests
{
    public class LeaveRequestCreateVM: IValidatableObject
    {
       
        [Required]
        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }

        [Required]
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }  // I only need the ID here, not to reference the entire LeaveType object(data model)


        [Display(Name = "Additional Infomartion")]
        [MaxLength(250, ErrorMessage = "The comment cannot exceed 250 characters.")]
        public string? RequestComment { get; set; } // This is the comment the employee can add when creating a leave request

        public IEnumerable<SelectListItem>?  LeaveTypesList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate > EndDate)
            {
                yield return new ValidationResult("The start date cannot be after the end date.", [nameof(StartDate), nameof(EndDate)]);
            }
        }
    }
}