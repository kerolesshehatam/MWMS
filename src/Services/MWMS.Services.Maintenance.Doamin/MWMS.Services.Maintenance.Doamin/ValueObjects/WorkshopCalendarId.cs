using System;
using System.Collections.Generic;
using System.Globalization;

namespace MWMS.Services.Maintenance.Doamin.ValueObjects
{
    public class WorkshopCalendarId : ValueObject
    {
        private const string DATE_FORMAT = "yyyy-MM-dd";
        public string Value { get; private set; }

        public static WorkshopCalendarId Create(DateTime date)
        {
            return new WorkshopCalendarId { Value = date.ToString(DATE_FORMAT) };
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        public static implicit operator string(WorkshopCalendarId id)
        {
            return id.Value;
        }

        public static implicit operator DateTime(WorkshopCalendarId id)
        {
            return DateTime.ParseExact(id.Value, DATE_FORMAT, CultureInfo.InvariantCulture);
        }
    }

}
