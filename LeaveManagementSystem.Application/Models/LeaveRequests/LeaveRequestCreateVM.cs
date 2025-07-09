using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Application.Models.LeaveRequests
{
    public class LeaveRequestCreateVM : IValidatableObject
    {

        [Required]
        [Display(Name = "Start Date")]
        public DateOnly StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateOnly EndDate { get; set; }

        // need the ID here, not the entire LeaveType object(data model)
        [Required]
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }


        [Display(Name = "Additional Infomartion")]
        [MaxLength(250, ErrorMessage = "The comment cannot exceed 250 characters.")]
        public string? RequestComment { get; set; }

        public IEnumerable<SelectListItem>? LeaveTypesList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate > EndDate)
            {
                yield return new ValidationResult("The start date cannot be after the end date.", [nameof(StartDate), nameof(EndDate)]);
            }
        }
    }
}