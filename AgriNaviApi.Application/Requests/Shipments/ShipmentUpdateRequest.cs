using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Shipments
{
    /// <summary>
    /// 出荷記録更新時のリクエスト
    /// </summary>
    public class ShipmentUpdateRequest
    {
        /// <summary>
        /// 日付
        /// </summary>
        [Display(Name = "日付")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public DateOnly ShipmentDate { get; set; }

        /// <summary>
        /// 圃場ID
        /// </summary>
        [Display(Name = "圃場ID")]
        [Range(CommonValidationRules.MinValidId, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int FieldId { get; set; }

        /// <summary>
        /// 作付ID
        /// </summary>
        [Display(Name = "作付ID")]
        [Range(CommonValidationRules.MinValidId, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int CropId { get; set; }

        /// <summary>
        /// 作付年間計画ID
        /// </summary>
        [Display(Name = "作付年間計画ID")]
        [Range(CommonValidationRules.MinValidId, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int SeasonScheduleId { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Display(Name = "備考")]
        [StringLength(CommonValidationRules.RemarkMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? Remark { get; set; }
    }
}
