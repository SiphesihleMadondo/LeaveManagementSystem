using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Web.Data
{
    // Entity - Data Model.
    public class LeaveType
    {
        // EF Core will automatically know this is a primary key // best practice.
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string Name { get; set; }

        [Precision(18, 2)]
        public decimal NumberOfDays { get; set; }
    }

}
