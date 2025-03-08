using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Users
{
    /// <summary>
    /// ユーザパスワード更新リクエスト
    /// </summary>
    public class PasswordUpdateRequest
    {
        /// <summary>
        /// ユーザID
        /// </summary>
        [Display(Name = "ユーザID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }

        /// <summary>
        /// パスワード
        /// </summary>
        [Display(Name = "パスワード")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(32, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
    }
}
