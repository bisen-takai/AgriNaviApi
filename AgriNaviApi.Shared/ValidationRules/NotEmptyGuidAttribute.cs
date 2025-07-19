using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Shared.ValidationRules
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotEmptyGuidAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                // null に対する検証は他の属性に任せる（[Required] 等）
                return true;
            }

            return value is Guid guid && guid != Guid.Empty;
        }


        public override string FormatErrorMessage(string name)
        {
            var errorMessage = ErrorMessageString;
            return string.Format(errorMessage, name);
        }
    }
}
