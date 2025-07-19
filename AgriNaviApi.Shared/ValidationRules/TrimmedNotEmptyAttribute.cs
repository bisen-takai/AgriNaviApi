using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgriNaviApi.Shared.ValidationRules
{
    public class TrimmedNotEmptyAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var str = value as string;

            // null や空文字は別の [Required] で弾く想定として、
            // ここでは「空でなく Trim 後に空でも NG」という判定を行う
            if (str is null || string.IsNullOrWhiteSpace(str.Trim()))
            {
                var errorMessage = FormatErrorMessage(validationContext.DisplayName ?? validationContext.MemberName!);
                return new ValidationResult(errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
