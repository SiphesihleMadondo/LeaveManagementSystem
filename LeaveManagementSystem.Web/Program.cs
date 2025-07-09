using LeaveManagementSystem.Application;
using LeaveManagementSystem.Application.Services.CurrentUser;
using LeaveManagementSystem.Application.Services.Email;
using LeaveManagementSystem.Application.Services.LeaveAllocations;
using LeaveManagementSystem.Application.Services.LeaveCalculationService;
using LeaveManagementSystem.Application.Services.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveTypes;
using LeaveManagementSystem.Application.Services.Periods;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


//adding the Services to the container
DataServicesRegistration.AddDataServices(builder.Services, builder.Configuration);
ApplicationServicesRegistration.AddApplicationServices(builder.Services);

builder.Host.UseSerilog((context, config) =>
{
    config.WriteTo.Console()
    .ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminSupervisorOnly", policy =>
    {
        policy.RequireRole(StaticRoles.Administrator, StaticRoles.Supervisor);

    });

});

builder.Services.AddHttpContextAccessor();


builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    // Configure identity options here if needed, e.g. password requirements, lockout settings, etc.
    options.SignIn.RequireConfirmedAccount = false; // Set to true if you want email confirmation
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
