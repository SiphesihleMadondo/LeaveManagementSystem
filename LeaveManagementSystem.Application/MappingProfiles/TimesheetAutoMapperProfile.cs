using AutoMapper;
using LeaveManagementSystem.Application.Models.Timesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Application.MappingProfiles
{
    public class TimesheetAutoMapperProfile : Profile
    {
        public TimesheetAutoMapperProfile()
        {
            // Define your mappings here
            // Example: CreateMap<SourceType, DestinationType>();
            CreateMap<EmployeeTimesheetVM, Timesheet>()
            .ForMember(dest => dest.WeekStartDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.WeekStartDate)))
            .ForMember(dest => dest.Entries, opt => opt.Ignore()) // Do custom mapping for entries
            .ForMember(dest => dest.Status, opt => opt.Ignore()); // if you're setting this manually

            CreateMap<Timesheet, TimesheetReadOnlyVM>()
                .ForMember(dest => dest.EmployeeFullName, opt => opt.MapFrom(src => src.Employee.FirstName + " " + src.Employee.LastName))
                .ForMember(dest => dest.WeekStartDate, opt => opt.MapFrom(src => src.WeekStartDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Entries, opt => opt.MapFrom(src => src.Entries.Select(e => new TimesheetEntryVM
                {
                    Date = e.Date.ToDateTime(TimeOnly.MinValue),
                    HoursWorked = e.HoursWorked,
                    TaskDescription = e.TaskDescription
                }).ToList()));

        }
    }
}
