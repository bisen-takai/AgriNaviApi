using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Users
{
    /// <summary>
    /// ユーザログインリクエスト
    /// </summary>
    public class UserLoginRequest
    {
        /// <summary>
        /// ログインID
        /// </summary>
        [Display(Name = "ユーザID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [TrimmedNotEmpty(ErrorMessageResourceName = nameof(CommonValidationMessages.InvalidFormatMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string LoginId { get; set; } = string.Empty;

        /// <summary>
        /// パスワード
        /// </summary>
        [Display(Name = "パスワード")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [TrimmedNotEmpty(ErrorMessageResourceName = nameof(CommonValidationMessages.InvalidFormatMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string Password { get; set; } = string.Empty;
    }
}
