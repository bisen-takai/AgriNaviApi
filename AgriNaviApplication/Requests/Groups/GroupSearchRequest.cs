using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Groups
{
    /// <summary>
    /// グループ検索リクエスト
    /// </summary>
    public class GroupSearchRequest
    {
        /// <summary>
        /// 検索グループ名
        /// </summary>
        [Display(Name = "検索グループ名")]
        public string? SearchGroupName { get; set; }

        /// <summary>
        /// グループ種別
        /// </summary>
        [Display(Name = "グループ種別")]
        [Range(0, 3, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public GroupKind? Kind { get; set; }

        /// <summary>
        /// 検索一致タイプ
        /// </summary>
        [Display(Name = "検索一致タイプ")]
        [Range((int)SearchMatchType.None, (int)SearchMatchType.PARTIAL, ErrorMessageResourceName = nameof(CommonValidationMessages.SearchMatchTypeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public SearchMatchType SearchMatchType { get; set; } = SearchMatchType.None;
    }
}
