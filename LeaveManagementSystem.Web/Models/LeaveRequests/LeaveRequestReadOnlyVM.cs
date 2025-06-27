using System.ComponentModel;
namespace LeaveManagementSystem.Web.Models.LeaveRequests
{
    public class LeaveRequestReadOnlyVM
    {

        public int Id { get; set; }

        [Display(Name = "StartDate")]
        public DateOnly StartDate { get; set; }
        [Display(Name = "EndDate")]
        public DateOnly EndDate { get; set; }

        [Display(Name = "Number Of Days")]
        public decimal NumberOfDays { get; set; }

        [Display(Name = "Leave Type")]
        public string LeaveTypeName { get; set; } = string.Empty;

        //leae request status
        [Display(Name = "Status")]
        public LeaveRequestStatusEnum LeaveRequestStatus { get; set; }
    }
}