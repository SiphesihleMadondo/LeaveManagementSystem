using System.Threading.Tasks;

namespace LeaveManagementSystem.Web.Services.LeaveCalculationService
{
    public class LeaveCalculatorService : ILeaveCalculatorService
    {
        // Standard working days per week
        private const decimal WorkingDaysPerWeek = 5;


        // Calculates annual leave entitlement.
        public async Task<decimal> CalculateAnnualLeaveDays(int monthsWorked)
        {
            // 1.25 days per month worked = 15 days per year
            var result = (decimal)Math.Floor(1.25 * monthsWorked);
            return await Task.FromResult(result);
        }

        // Calculates total sick leave entitlement in a 3-year cycle.
        public async Task<decimal> CalculateSickLeaveEntitlement(decimal workingDaysPerWeek = WorkingDaysPerWeek)
        {
            // Sick leave is equal to 6 weeks' worth of days over a 36-month period
            var result = workingDaysPerWeek * 6;
            return await Task.FromResult(result);
        }

        // Calculates the remaining sick leave after some days have been taken.
        public async Task<decimal> CalculateRemainingSickLeave(int totalEntitlement, int daysTaken)
        {
            var result = Math.Max(0, totalEntitlement - daysTaken);
            return await Task.FromResult(result);
        }

        // Returns the family responsibility leave entitlement (fixed at 3 per year).
        public decimal GetFamilyResponsibilityLeaveEntitlement() => 3;

        // Calculates remaining family responsibility leave.
        public async Task<decimal> CalculateRemainingFamilyLeave(decimal daysTaken)
        {
            var result = Math.Max(0, GetFamilyResponsibilityLeaveEntitlement() - daysTaken);
            return await Task.FromResult(result);
        }

        // Calculates special leave, typically a fixed number of days (e.g., 10 days).
        public async Task<decimal> CalculateSpecialLeave()
        {
            // Special leave is typically 10 days
            var result = 10;
            return await Task.FromResult(result);
        }

        //public async Task<decimal> CalculateMaternityLeave() 
        //{
        //    // Maternity leave is typically 4 months, which is 17 weeks or 85 working days
        //    // Assuming a standard 5-day work week
        //    var result = 85; // 17 weeks * 5 days per week
        //    return await Task.FromResult(result);
        //}
    }

}
