
using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Services.CurrentUser;
using LeaveManagementSystem.Web.Services.LeaveCalculationService;
using LeaveManagementSystem.Web.Services.Periods;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context 
       , IMapper _mapper, ICurrentUser _curreUser,
        IPeriodsService _period, ILeaveCalculatorService _leaveCalculator) : ILeaveAllocationsService
    {

        public int GetMonthsWorked(DateOnly dateOfJoining, DateOnly currentDate)
        {
            if (currentDate < dateOfJoining)
                return 0;

            int months = (currentDate.Year - dateOfJoining.Year) * 12 + currentDate.Month - dateOfJoining.Month;

            // If the current day is before the joining day, subtract a month (not a full month yet)
            if (currentDate.Day < dateOfJoining.Day)
            {
                months--;
            }

            return Math.Max(0, months); // ensure no negatives
        }


        public async Task AllocateLeave(string employeeId, int selectedLeaveTypeId)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now); ;

            //return the date of joining for the employee
            var dateOfJoining = await _context.Users
                .Where(x => x.Id == employeeId)
                .Select(x => x.DateOfJoining)
                .FirstOrDefaultAsync();

            // Get current period
            var currentPeriod = await _context.Periods
                .SingleAsync(x => x.EndDate.Year == dateOfJoining.Year);

            var monthsRemaining = GetMonthsWorked(dateOfJoining, currentDate);
          
            // Get all leave types
            var leaveTypes = await _context.LeaveTypes
                .Where(q => !q.leaveAllocations.Any(x => x.EmployeeId == employeeId && x.PeriodId == currentPeriod.Id))
                .ToListAsync();

            foreach (var leaveType in leaveTypes)
            {
                decimal days;

                // Calculate entitlement based on leave type name
                if (leaveType.Name.Contains("Sick", StringComparison.OrdinalIgnoreCase))
                {
                    days = await _leaveCalculator.CalculateSickLeaveEntitlement();
                }
                else if (leaveType.Name.Contains("Family", StringComparison.OrdinalIgnoreCase))
                {
                    days = _leaveCalculator.GetFamilyResponsibilityLeaveEntitlement();
                }
                else if(leaveType.Name.Contains("Special", StringComparison.OrdinalIgnoreCase))
                {
                    days = await _leaveCalculator.CalculateSpecialLeave();
                }
                else
                {
                    days = await _leaveCalculator.CalculateAnnualLeaveDays(monthsRemaining);
                }

                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    PeriodId = currentPeriod.Id,
                    Days = days
                };

                _context.LeaveAllocations.Add(leaveAllocation);
            }

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
            var user = string.IsNullOrEmpty(UserId)
                ? await _curreUser.GetCurrentUser()
                : await _curreUser.GetUserById(UserId);

            //get allocations (list), access them using GetEmployeeAllocations
            var allocations = await EmployeeAllocations(user.Id);

            //list of leave allocation into the list of Leave Allocation Vm
            //number of leaveTypes (is number of allocation eqaul to the number of leaveTypes int system)
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
            var users = await _curreUser.GetEmployeesAsync();
            var employees = _mapper.Map<List<ApplicationUser>, List<EmployeeListVM>>(users);

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
            .ExecuteUpdateAsync(x => x.SetProperty(e => e.Days, leaveAllocationEditVM.Days)); 
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

        
        public async Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId)
        {
            var period = await _period.GetCurrentPeriod();
            var allocation = await _context.LeaveAllocations
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.LeaveTypeId == leaveTypeId && a.PeriodId == period.Id);

            return allocation;
        }

        //check if there's already an allocation for the period
        public async Task<bool> AllocationExists(string? UserId, int periodId, int leaveTypeId)
        {
            var exists = await _context.LeaveAllocations.AnyAsync(x =>
            x.EmployeeId == UserId
            && x.PeriodId == periodId
            && x.LeaveTypeId == leaveTypeId);

            return exists;
        }

        



    }
}
