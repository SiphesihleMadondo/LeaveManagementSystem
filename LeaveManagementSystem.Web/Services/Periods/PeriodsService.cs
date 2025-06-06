using AutoMapper;
using LeaveManagementSystem.Web.Models.Periods;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.Periods
{
    public class PeriodsService(ApplicationDbContext _context, IMapper _mapper) : IPeriodsService
    {


        public async Task<List<PeriodReadOnlyVM>> GetResult()
        {
            var data = await _context.Periods.ToListAsync();
            var viewData = _mapper.Map<List<PeriodReadOnlyVM>>(data);
            return viewData;
        }

        public async Task<T?> GetT<T>(int? id) where T : class
        {
            var period = await _context.Periods.FirstOrDefaultAsync(m => m.Id == id);

            if (period == null)
            {
                return default;
            }

            var data = _mapper.Map<T>(period);
            return (T?)data;
        }

        public async Task Edit(PeriodEditVM editVM)
        {
            var data = _mapper.Map<Period>(editVM);
            _context.Update(data);
            await _context.SaveChangesAsync();

        }

        public async Task Create(PeriodCreateVM periodCreate)
        {
            var data = _mapper.Map<Period>(periodCreate);
            _context.Add(data);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var period = await _context.Periods.FindAsync(id);
            if (period != null)
            {
                _context.Periods.Remove(period);
            }
            await _context.SaveChangesAsync();
        }

        public bool PeriodExists(int id)
        {
            return _context.Periods.Any(e => e.Id == id);
        }
    }

}
