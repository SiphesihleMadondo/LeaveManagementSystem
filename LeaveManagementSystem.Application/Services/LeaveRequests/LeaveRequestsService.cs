using AutoMapper;
using LeaveManagementSystem.Application.Services.CurrentUser;
using LeaveManagementSystem.Application.Services.LeaveAllocations;
using LeaveManagementSystem.Application.Services.Periods;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Application.Services.LeaveRequests
{
    public class LeaveRequestsService(IMapper _mapper, ICurrentUser _currentUser, ApplicationDbContext _context,
        IPeriodsService _period, ILeaveAllocationsService _allocationsService) : ILeaveRequestsService
    {



        public async Task CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);
            //change the status to cancelled 
            leaveRequest.RequestStatusId = (int)LeaveRequestStatusEnum.Cancelled;

            // restore the allocation of days back to the employee's leave allocation
            await UpdateAllocationDays(leaveRequest, false);
            await _context.SaveChangesAsync();

        }

        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            //mapp data to the leave request data model
            var leaveRequest = _mapper.Map<LeaveRequest>(model);

            // get logged in employee ID from the current user context
            var user = await _currentUser.GetCurrentUser();
            leaveRequest.EmployeeId = user.Id;

            //set leaveRequest status to pending --> leaveRequest.Status = LeaveRequestStatus.Pending;
            leaveRequest.RequestStatusId = (int)LeaveRequestStatusEnum.Pending;
            _context.Add(leaveRequest);

            await UpdateAllocationDays(leaveRequest, true);
            await _context.SaveChangesAsync();

        }

        public async Task<EmployeeLeaveRequestListVM> AdminGetAllLeaveRequests()
        {
            //get all leave requests for the admin
            //LeaveType is a navigation property in LeaveRequest (Join)
            //Employee is a navigation property in LeaveRequest (Join)
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .ToListAsync();

            //this a convesion to the view model
            var leaveRequestsModels = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveTypeName = q.LeaveType?.Name,
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.RequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber, // Calculate the number of days
                EmployeeFirstName = q.Employee?.FirstName,
                EmployeeLastName = q.Employee?.LastName
            }).ToList();

            //use the list to do the calculation of the total number of requests and  return in a list
            var model = new EmployeeLeaveRequestListVM
            {
                TotalRequests = await _context.LeaveRequests.CountAsync(),
                ApprovedRequests = await _context.LeaveRequests.CountAsync(q => q.RequestStatusId == (int)LeaveRequestStatusEnum.Approved),
                PendingRequests = await _context.LeaveRequests.CountAsync(q => q.RequestStatusId == (int)LeaveRequestStatusEnum.Pending),
                RejectedRequests = await _context.LeaveRequests.CountAsync(q => q.RequestStatusId == (int)LeaveRequestStatusEnum.Rejected),

                LeaveRequests = leaveRequestsModels
            };

            return model;
        }

        public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests()
        {
            //find the allocation for the leave type and employee
            var user = await _currentUser.GetCurrentUser();
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .Where(q => q.EmployeeId == user.Id)
                .ToListAsync();
            var model = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveTypeName = q.LeaveType?.Name,
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.RequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber + 1 // Calculate the number of days

            }).ToList();
            return model; // Return the list of leave requests
        }

        public async Task ReviewLeaveRequest(int leaveRequestId, bool approved)
        {
            var user = await _currentUser.GetCurrentUser();
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);
            leaveRequest.RequestStatusId = approved ? (int)LeaveRequestStatusEnum.Approved : (int)LeaveRequestStatusEnum.Rejected;

            //update the id for the reviewer
            leaveRequest.ReviewerId = user.Id;

            //if rejected, restore the allocation of days back to the employee's leave allocation
            if (!approved)
            {
                await UpdateAllocationDays(leaveRequest, false);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int leaveRequestId)
        {
            //LeaveType is a navigation property in LeaveRequest (Join)
            var leaveRequest = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .FirstAsync(q => q.Id == leaveRequestId);

            //get the logged in user
            var user = await _currentUser.GetUserById(leaveRequest.EmployeeId);
            if (leaveRequest == null)
                return null;

            //wire up the model to the view model
            //Employee is a navigation property in LeaveRequest (Join)
            var model = new ReviewLeaveRequestVM
            {
                Id = leaveRequest.Id,
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber,
                LeaveTypeName = leaveRequest.LeaveType?.Name,
                RequestComments = leaveRequest.RequestComment,
                Employee = new EmployeeListVM
                {

                    Id = leaveRequest?.EmployeeId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email

                },
                LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequest.RequestStatusId
            };

            return model; // Return the view model with the leave request details
        }

        //pass in the LeaveRequest object which has all the info needed.
        public async Task UpdateAllocationDays(LeaveRequest leaveRequest, bool deductDays)
        {
            var allocation = await _allocationsService.GetCurrentAllocation(leaveRequest.LeaveTypeId, leaveRequest.EmployeeId);
            var numberOfDays = CalculateDays(leaveRequest.StartDate, leaveRequest.EndDate);
            var exists = await _allocationsService.AllocationExists(leaveRequest.EmployeeId, leaveRequest.LeaveTypeId, leaveRequest.StartDate.Year);

            // If the allocation exists and the allocation is not null, update the days
            if (exists && allocation != null)
            {
                allocation.Days += deductDays ? -numberOfDays : numberOfDays;
            }

            //the entity(allocation) has been changed and should be updated in the database when SaveChanges() is called.
            _context.Entry(allocation).State = EntityState.Modified;
        }

        private decimal CalculateDays(DateOnly start, DateOnly end)
        {
            return end.DayNumber - start.DayNumber;
        }

        //validation method to ensure the number of days requested does not exceed the allocation
        public async Task<bool> ValidateNumberOfDaysRequest(LeaveRequestCreateVM model)
        {

            //get period id for the leave request
            var period = await _period.GetCurrentPeriod();
            //calculate the number of days requested
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;

            //find the allocation for the leave type and employee
            var user = await _currentUser.GetCurrentUser();

            var allocation = await _context.LeaveAllocations
            .FirstOrDefaultAsync(a => a.EmployeeId == user.Id && a.LeaveTypeId == model.LeaveTypeId && a.PeriodId == period.Id);

            return allocation != null && allocation.Days < numberOfDays;
        }


    }
}
