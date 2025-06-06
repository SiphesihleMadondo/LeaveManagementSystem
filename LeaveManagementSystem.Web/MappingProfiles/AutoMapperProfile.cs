using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using LeaveManagementSystem.Web.Models.Periods;

namespace LeaveManagementSystem.Web.MappingProfiles
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile() {

            CreateMap<LeaveType, LeaveTypeReadOnlyVM>();
            //.ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.NumberOfDays)); //mapping from source to destination, perfomance issue though
            CreateMap<LeaveTypeCreateVM, LeaveType>();
            CreateMap<LeaveTypeEditVM, LeaveType>().ReverseMap();


            CreateMap<PeriodCreateVM, Period>();
            CreateMap<PeriodEditVM, Period>().ReverseMap();
        }
    }
}
