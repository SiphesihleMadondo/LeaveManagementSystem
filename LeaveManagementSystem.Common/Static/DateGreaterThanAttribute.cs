using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Common.Static
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _compare;

        public DateGreaterThanAttribute(string compare)
        {
            _compare = compare;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //assigning current value
            var currentValue = (DateOnly?)value;

            //getting the property
            var property = validationContext.ObjectType.GetProperty(_compare);

            //check if the property is null, if null return the error
            if (property == null)
            {
                return new ValidationResult($"Unknown property: {_compare}");
            }

            //assigning comparison value
            var comparisonValue = (DateOnly?)property.GetValue(validationContext.ObjectInstance);

            if (currentValue.HasValue && comparisonValue.HasValue && currentValue <= comparisonValue)
            {
                //returned message
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be after {_compare}");
            }

            return ValidationResult.Success;
        }
    }
}
