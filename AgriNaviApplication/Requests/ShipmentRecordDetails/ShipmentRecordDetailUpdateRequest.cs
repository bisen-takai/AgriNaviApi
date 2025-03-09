using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.ShipmentRecordDetails
{
    /// <summary>
    /// 出荷記録詳細更新リクエスト
    /// </summary>
    public class ShipmentRecordDetailUpdateRequest
    {
        /// <summary>
        /// 出荷記録ID
        /// </summary>
        [Display(Name = "出荷記録ID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }

        /// <summary>
        /// 出荷先ID
        /// </summary>
        [Display(Name = "出荷先ID")]
        public int ShippingDestinationId { get; set; }

        /// <summary>
        /// 品質規格ID
        /// </summary>
        [Display(Name = "品質規格ID")]
        public int QualityStandardId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "数量")]
        public int Quantity { get; set; }

        /// <summary>
        /// 単位ID
        /// </summary>
        [Display(Name = "単位ID")]
        public int UnitId { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        [Display(Name = "金額")]
        public int Amount { get; set; }
    }
}
