using AutoMapper;

namespace LeaveManagementSystem.Application.MappingProfiles
{
    public class LeaveRequestAutoMapperProfile : Profile
    {
        public LeaveRequestAutoMapperProfile()
        {

            CreateMap<LeaveRequestCreateVM, LeaveRequest>();

        }
    }
}
