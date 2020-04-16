using MWMS.Services.Maintenance.Doamin.Exceptions;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MWMS.Services.Maintenance.Doamin.ValueObjects
{
    public class LicenseNumber : ValueObject
    {
        private const string NUMBER_PATTERN = @"^[a-zA-Z0-9_.-]*$";

        public string Value { get; private set; }

        public static LicenseNumber Create(string value)
        {
            if (!Regex.IsMatch(value, NUMBER_PATTERN, RegexOptions.IgnoreCase))
            {
                throw new InvalidValueException($"The specified license-number '{value}' was not in the correct format.");
            }
            return new LicenseNumber { Value = value };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static implicit operator string(LicenseNumber licenseNumber)
        {
            return licenseNumber.Value;
        }
    }
}
