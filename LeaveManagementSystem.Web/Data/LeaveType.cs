using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Web.Data
{
    // Entity - Data Model.
    public class LeaveType: BaseEntity
    {

        [Column(TypeName = "nvarchar(150)")]
        public string Name { get; set; }

        [Precision(18, 2)]
        public decimal NumberOfDays { get; set; }
    }

}
