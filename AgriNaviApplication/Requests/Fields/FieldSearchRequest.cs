using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Fields
{
    /// <summary>
    /// 圃場検索リクエスト
    /// </summary>
    public class FieldSearchRequest
    {
        /// <summary>
        /// 検索圃場名
        /// </summary>
        [Display(Name = "検索圃場名")]
        public string? SearchFieldName { get; set; }

        /// <summary>
        /// グループID
        /// </summary>
        [Display(Name = "グループID")]
        public int? GroupId { get; set; }

        /// <summary>
        /// 検索一致タイプ
        /// </summary>
        [Display(Name = "検索一致タイプ")]
        [Range((int)SearchMatchType.None, (int)SearchMatchType.PARTIAL, ErrorMessageResourceName = nameof(CommonValidationMessages.SearchMatchTypeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public SearchMatchType SearchMatchType { get; set; } = SearchMatchType.None;
    }
}
