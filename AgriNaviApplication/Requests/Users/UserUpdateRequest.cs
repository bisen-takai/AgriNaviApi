using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Users
{
    /// <summary>
    /// ユーザ更新リクエスト(パスワード除く)
    /// </summary>
    public class UserUpdateRequest
    {
        /// <summary>
        /// ユーザID
        /// </summary>
        [Display(Name = "ユーザID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }

        /// <summary>
        /// ログインID
        /// </summary>
        [Display(Name = "ログインID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(20, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string LoginId { get; set; } = string.Empty;

        /// <summary>
        /// 氏名
        /// </summary>
        [Display(Name = "氏名")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(20, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? FullName { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        [RegularExpression(@"^\d{10,11}$", ErrorMessageResourceName = nameof(CommonValidationMessages.PhoneMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        [EmailAddress(ErrorMessageResourceName = nameof(CommonValidationMessages.EmailMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? Email { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        [StringLength(40, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? Address { get; set; }

        /// <summary>
        /// 権限ID
        /// </summary>
        public PrivilegeKind PrivilegeId { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        public int ColorId { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [StringLength(200, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? Remark { get; set; }
    }
}
