
using AutoMapper;
using LeaveManagementSystem.Web.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context, 
        IHttpContextAccessor _httpContextAccessor, UserManager<ApplicationUser> _userManager, IMapper _mapper, ICurrentUser _curreUser) : ILeaveAllocationsService
    {
        protected const decimal accruedDays = 1.25M;
        protected const int sickLeaveCycle = 3;
        protected const int sixweekLeave = 6;


        public async Task AllocateLeave(string employeeId, int selectedLeaveTypeId)
        {
            // Get the leave type by the provided ID, only if not already allocated to the employee.
            var leaveType = await _context.LeaveTypes
                .Where(q => q.Id == selectedLeaveTypeId &&
                            !q.leaveAllocations.Any(x => x.EmployeeId == employeeId))
                .FirstOrDefaultAsync();

            if (leaveType == null)
            {
                // Either the leave type doesn't exist or is already allocated for this employee.
                return;
            }

            // Get the current period based on the year
            var currentDate = DateTime.Now;
            var currentPeriod = await _context.Periods
                .SingleAsync(x => x.EndDate.Year == currentDate.Year);

            var monthsRemaining = currentPeriod.EndDate.Month - currentDate.Month;

            // Define constants (assuming these are defined somewhere else in your original context)
            const decimal sixweekLeave = 6.0M;
            const decimal sickLeaveCycle = 36.0M;
            const decimal accruedDays = 1.5M;

            decimal accrual = 0.0M;

            // Calculate accrual based on leave type name
            if (leaveType.Name.Contains("Sick"))
            {
                if (monthsRemaining <= 6)
                {
                    accrual = leaveType.NumberOfDays / 26;
                }
                else
                {
                    accrual = (leaveType.NumberOfDays * sixweekLeave) / sickLeaveCycle;
                }
            }
            else if (leaveType.Name.Contains("Family"))
            {
                accrual = monthsRemaining >= 4 ? 3.0M : 0.0M;
            }
            else
            {
                accrual = monthsRemaining * accruedDays;
            }

            // Create a new leave allocation
            var leaveAllocation = new LeaveAllocation
            {
                EmployeeId = employeeId,
                LeaveTypeId = leaveType.Id,
                PeriodId = currentPeriod.Id,
                Days = accrual
            };

            _context.Add(leaveAllocation);
            await _context.SaveChangesAsync();
        }



        //get the leave type id by name, this is used to get the leave type id for employee
        public async Task<int> GetLeaveTypeIdByNameAsync(string employee)
        {
            var leaveTypeId = await _context.LeaveTypes
                .Where(x => x.Name == employee)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            return leaveTypeId;
        }


        public async Task<EmployeeAllocationVM> GetEmployeeAllocations(string? UserId)
        {

            //get employee details elsewhere using a Ternary Operator
            var user = UserId.IsNullOrEmpty()
                ? await _curreUser.GetCurrentUser()
                : await _userManager.FindByIdAsync(UserId);

            //get allocations (list), access then using GetEmployeeAllocations
            var allocations = await EmployeeAllocations(user.Id);

            //list of leave allocation into the list of Leave Allocation Vm
            //number of leaveTypes (is number of allocation eqaul to the number of leaveTypes int system)
            //convert the list from domain object to vm object
            var allocanVMList =  _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations);
            var leaveTypes = await _context.LeaveTypes.CountAsync();

            var unAllocated = await _context.LeaveTypes
               .Where(q => !q.leaveAllocations.Any(x => x.EmployeeId == UserId))
               .ToListAsync(default);


            var employeeVM = new EmployeeAllocationVM()
            {
                DateOfBirth = user.DateOfBirth,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Id = user.Id,
                LeaveAllocations = allocanVMList,
                IsCompletedAllocation = leaveTypes == allocations.Count(), //
                UnAllocated = unAllocated
            };

            return employeeVM;
        }

        public async Task<List<EmployeeListVM>> GetemployeesAsync()
        {
            //get all the users that are employees.
            var users = await _userManager.GetUsersInRoleAsync(StaticRoles.Employee);
            var employees = _mapper.Map<List<ApplicationUser>, List<EmployeeListVM>>(users.ToList());

            return employees;
        }

        public async Task<LeaveAllocationEditVM> GetEmployeeAllocation(int AllocationId)
        {
            var allocation = await _context.LeaveAllocations
                .Include(q => q.LeaveType) // include records of LeaveType(Inner Join)
                .Include(q => q.Period)
                .Include(q => q.Employee) // include records of Employee (Inner Join)
                .FirstOrDefaultAsync(x => x.Id == AllocationId);

           var editAllocation =  _mapper.Map<LeaveAllocationEditVM>(allocation);
            if (editAllocation == null)
            {
                throw new Exception("Allocation not found");
            }
           
            
           return editAllocation;
        }
        public async Task UpdateLeaveAllocation(LeaveAllocationEditVM leaveAllocationEditVM)
        {
            //get the allocation from the VM
            //var allocation = await GetEmployeeAllocation(leaveAllocationEditVM.Id) ?? throw new Exception("Allocation does not exist.");
            //allocation.Days = leaveAllocationEditVM.Days;
            //option 1 _context.Update(allocation);
            //option 2 _context.Entry(allocation).State = EntityState.Modified; - this is the same as above but more explicit
            //_context.SaveChangesAsync() if wrorking with the two you to save the changes to the database

            //LAMDA Expression to update the allocation
            await _context.LeaveAllocations
            .Where(x => x.Id == leaveAllocationEditVM.Id)
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.Days, leaveAllocationEditVM.Days)); // this is the same as above but more explicit
            await _context.SaveChangesAsync();
        }

        //return a list of leaveAllocation objects
        private async Task<List<LeaveAllocation>> EmployeeAllocations(string? UserId)
        {

            //get all allocations for that particular employee for the current period
            var currentDate = DateTime.Now;
            var allocations = await _context.LeaveAllocations
                .Include(q => q.LeaveType) // include records of LeaveType(Inner Join)
                .Include(q => q.Period)
                .Where(q => q.EmployeeId == UserId && q.Period.EndDate.Year == currentDate.Year)
                .ToListAsync(); // this is what execute this query

            // now return the data
            return allocations;

        }

        //check if there's already an allocation for the period
        private async Task<bool> AllocationExists(string? UserId, int periodId, int leaveTypeId)
        {
            var exists = await _context.LeaveAllocations.AnyAsync(x =>
            x.EmployeeId == UserId
            && x.PeriodId == periodId
            && x.LeaveTypeId == leaveTypeId);

            return exists;
        }

        



    }
}
