using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Crops
{
    /// <summary>
    /// 作付更新時のリクエスト
    /// </summary>
    public class CropUpdateRequest
    {
        /// <summary>
        /// 作付名
        /// </summary>
        [Display(Name = "作付名")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(CropValidationRules.NameMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [TrimmedNotEmpty(ErrorMessageResourceName = nameof(CommonValidationMessages.InvalidFormatMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 作付名省略名
        /// </summary>
        [Display(Name = "作付名省略名")]
        [StringLength(CommonValidationRules.ShortNameMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? ShortName { get; set; }

        /// <summary>
        /// グループID
        /// </summary>
        [Display(Name = "グループID")]
        [Range(CommonValidationRules.MinValidId, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int GroupId { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        [Display(Name = "カラーID")]
        [Range(CommonValidationRules.MinValidId, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int ColorId { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [StringLength(CommonValidationRules.RemarkMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? Remark { get; set; }
    }
}
