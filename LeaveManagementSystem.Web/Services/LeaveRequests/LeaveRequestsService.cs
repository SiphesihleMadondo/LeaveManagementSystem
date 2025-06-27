using AutoMapper;
using LeaveManagementSystem.Web.Services.CurrentUser;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public class LeaveRequestsService(IMapper _mapper, ICurrentUser _currentUser, ApplicationDbContext _context): ILeaveRequestsService
    {
        public async Task CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);
            //change the status to cancelled use the tracking method by ef core
            leaveRequest.RequestStatusId = (int)LeaveRequestStatusEnum.Cancelled; // Assuming LeaveRequestStatus is an enum

            // restore the allocation of days back to the employee's leave allocation
            var user = await _currentUser.GetCurrentUser(); // Using the injected ICurrentUser service
            //deduct number of days upon request
            var numberOfdays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber;

            //get the allocation record for the employee and leave type
            var allocation = await _context.LeaveAllocations
                .FirstOrDefaultAsync(a => a.EmployeeId == user.Id && a.LeaveTypeId == leaveRequest.LeaveTypeId);

            //add back the number of days from the allocation
            if (allocation != null)
            {
                allocation.Days += numberOfdays;
            }

            if (leaveRequest != null) 
            {
                await _context.SaveChangesAsync(); // Save changes to the database
            }

        }

        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            //mapp data to the leave request data model
            var leaveRequestStatus =  _mapper.Map<LeaveRequest>(model);
            // get logged in employee ID from the current user context
            var user = await _currentUser.GetCurrentUser(); // Using the injected ICurrentUser service
            leaveRequestStatus.EmployeeId = user.Id; // Assuming the current user has an Id property

            //set leaveRequest status to pending
            // leaveRequest.Status = LeaveRequestStatus.Pending;
            leaveRequestStatus.RequestStatusId = (int)LeaveRequestStatusEnum.Pending; //LeaveRequestStatus is an enum
            _context.Add(leaveRequestStatus); // Add the leave request to the context

      
            //deduct number of days upon request
            var numberOfdays = model.EndDate.DayNumber - model.StartDate.DayNumber;

            //get the allocation record for the employee and leave type
            var allocationDeduct = await _context.LeaveAllocations
                .FirstOrDefaultAsync(a => a.EmployeeId == user.Id && a.LeaveTypeId == model.LeaveTypeId);

            //minus the number of days from the allocation
            if (allocationDeduct != null)
            {
                allocationDeduct.Days -= numberOfdays;
                _context.Update(allocationDeduct); // Update the allocation record in the context
                await _context.SaveChangesAsync(); // Save changes to the database
            }
        }

        public Task<List<EmployeeLeaveRequestListVM>> GetAllLeaveRequests()
        {
            throw new NotImplementedException();
        }

        public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests()
        {
            //find the allocation for the leave type and employee
            var user = await _currentUser.GetCurrentUser(); // Using the injected ICurrentUser service
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType) // Assuming LeaveType is a navigation property in LeaveRequest (Join)
                .Where(q => q.EmployeeId == user.Id)
                .ToListAsync();
           var model = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
           { 
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveTypeName = q.LeaveType?.Name, // Assuming LeaveType is a navigation property in LeaveRequest (Join)
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.RequestStatusId, // Assuming RequestStatusId is an int that maps to the enum
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber // Calculate the number of days

           }).ToList();
            return model; // Return the list of leave requests mapped to the view model
        }

        public Task ReviewLeaveRequest(ReviewLeaveRequestVM reviewRequest)
        {
            throw new NotImplementedException();
        }

        //validation method to ensure the number of days requested does not exceed the allocation
        public async Task<bool> ValidateNumberOfDaysRequest(LeaveRequestCreateVM model)
        {
            //first calculate the number of days requested
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;

            //find the allocation for the leave type and employee
            var user = await _currentUser.GetCurrentUser(); // Using the injected ICurrentUser service
   
            var allocation = await _context.LeaveAllocations
            .FirstOrDefaultAsync(a => a.EmployeeId == user.Id && a.LeaveTypeId == model.LeaveTypeId);

            return allocation != null && allocation.Days < numberOfDays;
        }
    }
}
