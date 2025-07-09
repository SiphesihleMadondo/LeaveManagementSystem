using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LeaveManagementSystem.Data
{
    public static class DataServicesRegistration
    {
        //is an extension method for IServiceCollection to register data services
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the (IoC)container.
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();
            return services;
        }
    }
}
