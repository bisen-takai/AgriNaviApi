using AgriNaviApi.Shared.Enums;
using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.ShipmentLines
{
    /// <summary>
    /// 出荷記録詳細検索時のリクエスト
    /// </summary>
    public class ShipmentLineSearchRequest
    {
        /// <summary>
        /// 出荷記録ID
        /// </summary>
        [Display(Name = "出荷記録ID")]
        [Range(CommonValidationRules.MinValidId, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int? ShipmentId { get; set; }

        /// <summary>
        /// 出荷先ID
        /// </summary>
        [Display(Name = "出荷先ID")]
        [Range(CommonValidationRules.MinValidId, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int? ShipDestinationId { get; set; }

        /// <summary>
        /// 品質規格ID
        /// </summary>
        [Display(Name = "品質規格ID")]
        [Range(CommonValidationRules.MinValidId, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int? QualityStandardId { get; set; }

        /// <summary>
        /// ページ番号（1始まり）
        /// </summary>
        [Display(Name = "ページ番号")]
        [Range(1, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Page { get; set; } = 1;

        /// <summary>
        /// 1ページあたりの件数
        /// </summary>
        [Display(Name = "件数")]
        [Range(CommonValidationRules.PageSizeMin, CommonValidationRules.PageSizeMax, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// ソートキー
        /// </summary>
        [Display(Name = "ソートキー")]
        [EnumDataType(typeof(ShipmentLineSortKey), ErrorMessageResourceName = nameof(CommonValidationMessages.EnumSortKeyMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public ShipmentLineSortKey? SortBy { get; set; }

        /// <summary>
        /// 降順ソートかどうか
        /// </summary>
        [Display(Name = "降順ソートか")]
        public bool SortDescending { get; set; } = false;
    }
}
