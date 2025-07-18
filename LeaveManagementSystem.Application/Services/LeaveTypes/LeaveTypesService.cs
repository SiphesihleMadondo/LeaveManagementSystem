﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LeaveManagementSystem.Application.Services.LeaveTypes;

public class LeaveTypesService(ApplicationDbContext _context, IMapper _mapper, ILogger<LeaveTypesService> _logger) : ILeaveTypesService
{
    public async Task<List<LeaveTypeReadOnlyVM>> GetAll()
    {
        // var data = SELECT * FROM LeaveTypes
        var data = await _context.LeaveTypes.ToListAsync();
        //convert data model to into a view model - using Auto Mapper
        var viewData = _mapper.Map<List<LeaveTypeReadOnlyVM>>(data);
        return viewData;
    }

    public async Task<T?> Get<T>(int id) where T : class
    {
        var data = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.Id == id);

        if (data == null)
        {

            return default;
        }

        var viewData = _mapper.Map<T>(data);
        return (T?)viewData;
    }

    public async Task Remove(int id)
    {
        var data = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.Id == id);
        if (data != null)
        {

            _context.Remove(data);
            await _context.SaveChangesAsync();
        }
    }

    public async Task Edit(LeaveTypeEditVM model)
    {
        var leaveType = _mapper.Map<LeaveType>(model);
        _context.Update(leaveType);
        await _context.SaveChangesAsync();
    }

    public async Task Create(LeaveTypeCreateVM model)
    {
        _logger.LogInformation("Creating a new leave type: {Name} - {Days}", model.Name, model.NumberOfDays);
        // mapp to LeaveType data that is found on this VM --> leaveCreateVM
        var leaveType = _mapper.Map<LeaveType>(model);
        _context.Add(leaveType);
        await _context.SaveChangesAsync();
    }

    // Checks if the record exist, used in the edit action
    public async Task<bool> LeaveTypeExists(int id)
    {
        return await _context.LeaveTypes.AnyAsync(e => e.Id == id);
    }

    //check if the name exists
    public async Task<bool> CheckIfLeaveTypeExistAsync(string name)
    {
        var _lowerName = name.ToLower();
        return await _context.LeaveTypes.AnyAsync(m => m.Name.ToLower().Equals(_lowerName));
    }

    //check- if record with the same name and id exist
    public async Task<bool> CheckIfLeaveTypeExistForEditAsync(LeaveTypeEditVM leaveTypeEditVM)
    {
        var _lowerName = leaveTypeEditVM.Name.ToLower();
        return await _context.LeaveTypes.AnyAsync(m => m.Name.ToLower().Equals(_lowerName) && m.Id != leaveTypeEditVM.Id);
    }
    //check maximum number of days for the leave type
    public async Task<bool> IsLeaveTypeValid(int leaveTypeId, decimal days)
    {
        var leaveType = await _context.LeaveTypes
            .Where(x => x.Id == leaveTypeId && x.NumberOfDays < days)
            .FirstOrDefaultAsync();
        return leaveType != null;
    }

}

