using AgriNaviApi.Shared.Enums;
using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Users
{
    /// <summary>
    /// ユーザ登録時のリクエスト
    /// </summary>
    public class UserCreateRequest
    {
        /// <summary>
        /// ログインID
        /// </summary>
        [Display(Name = "ログインID")]
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

        /// <summary>
        /// 氏名
        /// </summary>
        [Display(Name = "氏名")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(UserValidationRules.FullNameMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [TrimmedNotEmpty(ErrorMessageResourceName = nameof(CommonValidationMessages.InvalidFormatMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? FullName { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        [Display(Name = "電話番号")]
        [RegularExpression(UserValidationRules.PhoneNumberPattern, ErrorMessageResourceName = nameof(UserValidationMessages.PhoneMessage), ErrorMessageResourceType = typeof(UserValidationMessages))]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        [Display(Name = "メールアドレス")]
        [EmailAddress(ErrorMessageResourceName = nameof(UserValidationMessages.EmailMessage), ErrorMessageResourceType = typeof(UserValidationMessages))]
        public string? Email { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        [Display(Name = "住所")]
        [StringLength(UserValidationRules.AddressMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? Address { get; set; }

        /// <summary>
        /// 権限ID
        /// </summary>
        [Display(Name = "権限ID")]
        [EnumDataType(typeof(PrivilegeKind), ErrorMessageResourceName = nameof(UserValidationMessages.PrivilegeKindMessage), ErrorMessageResourceType = typeof(UserValidationMessages))]
        public PrivilegeKind PrivilegeId { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        [Display(Name = "カラーID")]
        [Range(CommonValidationRules.MinValidId, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int ColorId { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Display(Name = "備考")]
        [StringLength(CommonValidationRules.RemarkMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? Remark { get; set; }
    }
}
