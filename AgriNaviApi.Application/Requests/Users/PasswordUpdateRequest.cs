using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Users
{
    public class PasswordUpdateRequest
    {
        /// <summary>
        /// ユーザID
        /// </summary>
        [Display(Name = "ユーザID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(UserValidationRules.LoginIdMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [TrimmedNotEmpty(ErrorMessageResourceName = nameof(CommonValidationMessages.InvalidFormatMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string LoginId { get; set; } = string.Empty;

        /// <summary>
        /// パスワード
        /// </summary>
        [Display(Name = "パスワード")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(UserValidationRules.PasswordNumMax, MinimumLength = UserValidationRules.PasswordNumMin, ErrorMessageResourceName = nameof(UserValidationMessages.PasswordRangeMessage), ErrorMessageResourceType = typeof(UserValidationMessages))]
        [TrimmedNotEmpty(ErrorMessageResourceName = nameof(CommonValidationMessages.InvalidFormatMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string Password { get; set; } = string.Empty;
    }
}
