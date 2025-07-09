using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Data
{
    // Entity - Data Model.
    public class LeaveType : BaseEntity
    {

        [Column(TypeName = "nvarchar(150)")]
        public string Name { get; set; }

        [Precision(18, 2)]
        public decimal NumberOfDays { get; set; }
        // get the allocations related to this leaveType, doing the reverse getting all the related leave allocations
        public List<LeaveAllocation>? leaveAllocations { get; set; }
    }

}
