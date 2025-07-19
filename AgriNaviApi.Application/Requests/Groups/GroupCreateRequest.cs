using AgriNaviApi.Shared.Enums;
using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Groups
{
    /// <summary>
    /// グループ登録時のリクエスト
    /// </summary>
    public class GroupCreateRequest
    {
        /// <summary>
        /// グループ名
        /// </summary>
        [Display(Name = "グループ名")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(GroupValidationRules.NameMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [TrimmedNotEmpty(ErrorMessageResourceName = nameof(CommonValidationMessages.InvalidFormatMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// グループ種別
        /// </summary>
        [Display(Name = "グループ種別")]
        [EnumDataType(typeof(GroupKind), ErrorMessageResourceName = nameof(GroupValidationMessages.GroupKindMessage), ErrorMessageResourceType = typeof(GroupValidationMessages))]
        public GroupKind Kind { get; set; }
    }
}
