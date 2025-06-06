
using AutoMapper;
using LeaveManagementSystem.Web.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context, 
        IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager, IMapper _mapper, ICurreUser _curreUser) : ILeaveAllocationsService
    {
        protected const decimal accruedDays = 1.25M;
        protected const int sickLeaveCycle = 3;
        protected const int sixweekLeave = 6;

       
        public async Task AllocateLeave(string employeeId)
        {
            // get all the leaveTypes
            var leaveTypes = await _context.LeaveTypes.ToListAsync(default);

            //--> get the current period based on the year, end date year == current date year
            var currentDate = DateTime.Now;
            var currentPeriod = await _context.Periods.SingleAsync(x => x.EndDate.Year == currentDate.Year);
            var monthsRemaining = currentPeriod.EndDate.Month - currentDate.Month;

           

       
            //foreach leave type, create an allocation entry

            foreach (var leaveType in leaveTypes) {

                var accrual = 0.0M;
              
                if (leaveType.Name.Contains("Sick"))
                {
                    // First 6 Months of Employment
                    if (monthsRemaining <= 6)
                    {
                        accrual = leaveType.NumberOfDays / 26;
                    }
                    else
                    {
                        // Sick Leave Per Year
                        accrual = (leaveType.NumberOfDays * sixweekLeave ) / sickLeaveCycle;
                    }
                }
                else if (leaveType.Name.Contains("Family"))
                {
                    if(monthsRemaining >= 4)
                    {
                        accrual = 3.00M; // family has no accrual just granted full at once
                    }
                    else
                    {
                        accrual = 0;
                    }
                }
                else
                {
                    //accrual per month
                    accrual = monthsRemaining * accruedDays;
                }

                //create a new instance of leaveAllocation
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    PeriodId = currentPeriod.Id,
                    //calculate leave days.
                    Days = accrual

                };

                // we need to add leaveAllocation to the database. Add method is used for trecking so it knows the object needs to be saved.
                _context.Add(leaveAllocation);
               

            }
            //after adding all the records then save them at once
            await _context.SaveChangesAsync();

        }

        //return a list of leaveAllocation objects
        public async Task<List<LeaveAllocation>> EmployeeAllocations()
        {
            //need to find a logged in user and fetch the user that has this username
            var user = await _curreUser.GetCurrentUser();

            //get all allocations for that particular employee for the current period
            var currentDate = DateTime.Now;
            var allocations = await _context.LeaveAllocations
                .Include(q => q.LeaveType) // include records of LeaveType(Inner Join)
                .Include(q => q.Period)
                .Where(q => q.EmployeeId == user.Id && q.Period.EndDate.Year == currentDate.Year)
                .ToListAsync(); // this is what execute this query

            // now return the data
            return allocations;

        }

        public async Task<EmployeeAllocationVM> GetEmployeeAllocations()
        {
            //get allocations (list), access then using GetEmployeeAllocations
            var allocations = await EmployeeAllocations();

            //list of leave allocation into the list of Leave Allocation Vm
            //convert the list from domain object to vm object
            var allocanVMList =  _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations);

            //get employee details elsewhere
            var user = await _curreUser.GetCurrentUser();
            var employeeVM = new EmployeeAllocationVM()
            {
                DateOfBirth = user.DateOfBirth,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Id = user.Id,
                LeaveAllocations = allocanVMList
            };

            return employeeVM;
        }

    }
}
