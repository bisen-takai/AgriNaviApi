using AgriNaviApi.Shared.Enums;
using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.ShipmentWithLines
{
    /// <summary>
    /// 出荷記録検索時のリクエスト
    /// </summary>
    [DateRangeValidation("StartDate", "EndDate")]
    public class ShipmentWithLineSearchRequest
    {
        /// <summary>
        /// 年間作付計画ID
        /// </summary>
        [Display(Name = "年間作付計画ID")]
        public int SeasonScheduleId { get; set; }

        /// <summary>
        /// 出荷記録開始日
        /// </summary>
        [Display(Name = "出荷記録開始日")]
        public DateOnly? StartDate { get; set; }

        /// <summary>
        /// 出荷記録終了日
        /// </summary>
        [Display(Name = "出荷記録終了日")]
        public DateOnly? EndDate { get; set; }

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
        [EnumDataType(typeof(ShipmentSortKey), ErrorMessageResourceName = nameof(CommonValidationMessages.EnumSortKeyMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public ShipmentSortKey? SortBy { get; set; }

        /// <summary>
        /// 降順ソートかどうか
        /// </summary>
        [Display(Name = "降順ソートか")]
        public bool SortDescending { get; set; } = false;
    }
}
