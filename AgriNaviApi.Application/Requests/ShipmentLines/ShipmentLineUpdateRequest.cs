using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.ShipmentLines
{
    /// <summary>
    /// 出荷記録詳細更新時のリクエスト
    /// </summary>
    public class ShipmentLineUpdateRequest
    {
        /// <summary>
        /// 出荷記録ID
        /// </summary>
        [Display(Name = "出荷記録ID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [Range(CommonValidationRules.MinValidId, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int ShipmentId { get; set; }

        /// <summary>
        /// 出荷先ID
        /// </summary>
        [Display(Name = "出荷先ID")]
        [Range(CommonValidationRules.MinValidId, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int ShipDestinationId { get; set; }

        /// <summary>
        /// 品質規格ID
        /// </summary>
        [Display(Name = "品質規格ID")]
        [Range(CommonValidationRules.MinValidId, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int QualityStandardId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "数量")]
        [Range(ShipmentLineValidationRules.QuantityMin, ShipmentLineValidationRules.QuantityMax, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Quantity { get; set; }

        /// <summary>
        /// 単位ID
        /// </summary>
        [Display(Name = "単位ID")]
        [Range(CommonValidationRules.MinValidId, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int UnitId { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        [Display(Name = "金額")]
        [Range(ShipmentLineValidationRules.AmountMin, ShipmentLineValidationRules.AmountMax, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Amount { get; set; }
    }
}
