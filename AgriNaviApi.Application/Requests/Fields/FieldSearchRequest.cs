using AgriNaviApi.Shared.Enums;
using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Fields
{
    /// <summary>
    /// 圃場検索時のリクエスト
    /// </summary>
    public class FieldSearchRequest
    {
        /// <summary>
        /// 検索名
        /// </summary>
        [Display(Name = "検索名")]
        [StringLength(FieldValidationRules.NameMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? SearchName { get; set; }

        /// <summary>
        /// 検索一致タイプ
        /// </summary>
        [Display(Name = "検索一致タイプ")]
        [EnumDataType(typeof(SearchMatchType), ErrorMessageResourceName = nameof(CommonValidationMessages.SearchMatchTypeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public SearchMatchType SearchMatchType { get; set; } = SearchMatchType.None;

        /// <summary>
        /// グループID
        /// </summary>
        [Display(Name = "グループID")]
        [Range(CommonValidationRules.MinValidId, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int? GroupId { get; set; }

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
        [EnumDataType(typeof(FieldSortKey), ErrorMessageResourceName = nameof(CommonValidationMessages.EnumSortKeyMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public FieldSortKey? SortBy { get; set; }

        /// <summary>
        /// 降順ソートかどうか
        /// </summary>
        [Display(Name = "降順ソートか")]
        public bool SortDescending { get; set; } = false;
    }
}
