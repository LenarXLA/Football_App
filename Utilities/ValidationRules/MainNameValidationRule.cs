using System;
using System.Globalization;
using System.Windows.Controls;

namespace Football_App.Utilities.ValidationRules
{
    public class MainNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Поле не должно быть пустым!");
        }
    }
}