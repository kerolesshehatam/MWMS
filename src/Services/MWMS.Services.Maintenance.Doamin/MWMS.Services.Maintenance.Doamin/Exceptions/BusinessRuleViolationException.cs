using System;

namespace MWMS.Services.Maintenance.Doamin.Exceptions
{
    public class BusinessRuleViolationException : Exception
    {
        public BusinessRuleViolationException()
        {
        }

        public BusinessRuleViolationException(string message) : base(message)
        {
        }

        public BusinessRuleViolationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
