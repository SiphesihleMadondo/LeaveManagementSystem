namespace LeaveManagementSystem.Application.Services.LeaveCalculationService
{
    public interface ILeaveCalculatorService
    {
        Task<decimal> CalculateAnnualLeaveDays(int monthsWorked);
        Task<decimal> CalculateRemainingFamilyLeave(decimal daysTaken);
        Task<decimal> CalculateRemainingSickLeave(int totalEntitlement, int daysTaken);
        Task<decimal> CalculateSickLeaveEntitlement(decimal workingDaysPerWeek = 5);
        Task<decimal> CalculateSpecialLeave();
        decimal GetFamilyResponsibilityLeaveEntitlement();
    }
}