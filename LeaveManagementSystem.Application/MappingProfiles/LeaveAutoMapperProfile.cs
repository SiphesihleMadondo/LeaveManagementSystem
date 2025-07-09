using AutoMapper;

namespace LeaveManagementSystem.Application.MappingProfiles
{
    public class LeaveAutoMapperProfile : Profile
    {
        public LeaveAutoMapperProfile()
        {

            CreateMap<LeaveType, LeaveTypeReadOnlyVM>();
            //.ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.NumberOfDays)); //mapping from source to destination, perfomance issue
            CreateMap<LeaveTypeCreateVM, LeaveType>();
            CreateMap<LeaveTypeEditVM, LeaveType>().ReverseMap();


            CreateMap<PeriodCreateVM, Period>();
            CreateMap<PeriodEditVM, Period>().ReverseMap();
        }
    }
}
