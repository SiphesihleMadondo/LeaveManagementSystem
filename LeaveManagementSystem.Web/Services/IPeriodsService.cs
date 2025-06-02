using LeaveManagementSystem.Web.Models.Periods;

namespace LeaveManagementSystem.Web.Services
{
    public interface IPeriodsService
    {
        Task Create(PeriodCreateVM periodCreate);
        Task Delete(int id);
        Task Edit(PeriodEditVM editVM);
        Task<List<PeriodReadOnlyVM>> GetResult();
        Task<T?> GetT<T>(int? id) where T : class;
        bool PeriodExists(int id);
    }
}