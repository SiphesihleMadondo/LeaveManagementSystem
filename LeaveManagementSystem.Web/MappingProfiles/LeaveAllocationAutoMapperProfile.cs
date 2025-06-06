using AutoMapper;
using LeaveManagementSystem.Web.Models.Periods;


namespace LeaveManagementSystem.Web.MappingProfiles
{
    public class LeaveAllocationAutoMapperProfile: Profile
    {
        public LeaveAllocationAutoMapperProfile()
        {
            CreateMap<LeaveAllocation, LeaveAllocationVM>();
            CreateMap<Period, PeriodReadOnlyVM>();
          
        }
    }
}
