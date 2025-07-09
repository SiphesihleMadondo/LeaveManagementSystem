using LeaveManagementSystem.Application.Services.CurrentUser;
using LeaveManagementSystem.Application.Services.Email;
using LeaveManagementSystem.Application.Services.LeaveAllocations;
using LeaveManagementSystem.Application.Services.LeaveCalculationService;
using LeaveManagementSystem.Application.Services.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveTypes;
using LeaveManagementSystem.Application.Services.Periods;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Application
{
    public static class ApplicationServicesRegistration
    {
        //is an extension method for IServiceCollection to register application services
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register application services here
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ILeaveTypesService, LeaveTypesService>(); //// letting IoC know about the service layer and interface
            services.AddScoped<IPeriodsService, PeriodsService>();
            services.AddScoped<ILeaveAllocationsService, LeaveAllocationsService>();
            services.AddScoped<ILeaveRequestsService, LeaveRequestsService>();
            services.AddScoped<ILeaveCalculatorService, LeaveCalculatorService>();
            services.AddScoped<ICurrentUser, CurrentUserService>();
            services.AddTransient<IEmailSender, EmailSender>(); // we want new instant everytime we run the app
            return services;
        }
    }
}
