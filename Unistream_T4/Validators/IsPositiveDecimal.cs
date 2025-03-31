using System.ComponentModel.DataAnnotations;

namespace Unistream_T4.Validators
{
    public class IsPositiveDecimalAttribute : ValidationAttribute
    {

        public IsPositiveDecimalAttribute()
        {
            
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return false;
            return Convert.ToDecimal(value) > 0m;
        }
    }
}
