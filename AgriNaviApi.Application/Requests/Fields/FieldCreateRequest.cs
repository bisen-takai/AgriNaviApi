using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Fields
{
    /// <summary>
    /// 圃場登録時のリクエスト
    /// </summary>
    public class FieldCreateRequest
    {
        /// <summary>
        /// 圃場名
        /// </summary>
        [Display(Name = "圃場名")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(FieldValidationRules.NameMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [TrimmedNotEmpty(ErrorMessageResourceName = nameof(CommonValidationMessages.InvalidFormatMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 圃場名省略名
        /// </summary>
        [Display(Name = "圃場名省略名")]
        [StringLength(CommonValidationRules.ShortNameMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? ShortName { get; set; }

        /// <summary>
        /// 面積(m2)
        /// </summary>
        [Display(Name = "面積")]
        [Range(typeof(decimal), FieldValidationRules.AreaM2Min, FieldValidationRules.AreaM2Max, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public decimal? FieldSize { get; set; }

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
