using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Unistream_T4.Validators
{
    public class NotFutureDateAttribute : ValidationAttribute
    {
        public NotFutureDateAttribute()
        {
            
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return false;

            DateTime? valueAsDateTime = value switch
            {
                DateTimeOffset => ((DateTimeOffset)value).UtcDateTime,
                DateTime => ((DateTime)value).ToUniversalTime(),
                _ => null
            };
            if (valueAsDateTime==null) return false;
            
            return (valueAsDateTime <= DateTime.UtcNow);
        }

    }
}
