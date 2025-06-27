using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    public class LeaveTypeReadOnlyVM: BaseLeaveTypeVM
    {

        public string Name { get; set; } = string.Empty;

        [Display(Name = "Maximum Allocation of Days")]
        public decimal NumberOfDays { get; set; }

    }

}
