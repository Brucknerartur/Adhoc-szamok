using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Adhoc_szamok
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? text = value as string;
            if (string.IsNullOrWhiteSpace(text))
                return new ValidationResult(false, "Nem lehet üres.");
            return ValidationResult.ValidResult;
        }
    }
    public class LenghtValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string? text = value as string;
            if (string.IsNullOrWhiteSpace(text))
                return new ValidationResult(false, "Nem lehet üres.");

            var split = text.Split(':');
            if (split.Length != 2 ||
                !int.TryParse(split[0], out _) ||
                !int.TryParse(split[1], out _))
            {
                return new ValidationResult(false, "Hibás formátum. Pl: 00:00");
            }

            return ValidationResult.ValidResult;
        }
    }
    public class RequiredDateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || !(value is DateTime))
                return new ValidationResult(false, "Kötelező mező");

            return ValidationResult.ValidResult;
        }
    }

    public class NumericValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            // Ha null vagy üres, hibát jelez
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult(false, "A mező nem lehet üres.");
            }

            // Ellenőrizzük, hogy szám-e
            if (!double.TryParse(value.ToString(), out _))
            {
                return new ValidationResult(false, "Csak számok engedélyezettek.");
            }

            // Ha minden rendben, valid
            return ValidationResult.ValidResult;
        }
    }
}
