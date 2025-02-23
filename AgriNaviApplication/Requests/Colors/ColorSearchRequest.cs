using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Colors
{
    /// <summary>
    /// カラー検索リクエスト
    /// </summary>
    public class ColorSearchRequest
    {
        /// <summary>
        /// 検索カラー名
        /// </summary>
        [Display(Name = "検索カラー名")]
        public string? SearchColorName { get; set; }

        /// <summary>
        /// 検索RGB値
        /// </summary>
        [Display(Name = "検索RGB値")]
        [Range(0,16777215, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int RGB { get; set; }

        /// <summary>
        /// 検索一致タイプ
        /// </summary>
        [Display(Name = "検索一致タイプ")]
        [Range((int)SearchMatchType.None, (int)SearchMatchType.PARTIAL, ErrorMessageResourceName = nameof(CommonValidationMessages.SearchMatchTypeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public SearchMatchType SearchMatchType { get; set; } = SearchMatchType.None;
    }
}
