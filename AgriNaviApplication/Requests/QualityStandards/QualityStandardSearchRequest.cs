using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.QualityStandards
{
    /// <summary>
    /// 品質・規格検索リクエスト
    /// </summary>
    public class QualityStandardSearchRequest
    {
        /// <summary>
        /// 検索品質・規格名
        /// </summary>
        [Display(Name = "検索品質・規格名")]
        public string? SearchQualityStandardName { get; set; }

        /// <summary>
        /// 検索一致タイプ
        /// </summary>
        [Display(Name = "検索一致タイプ")]
        [Range((int)SearchMatchType.None, (int)SearchMatchType.PARTIAL, ErrorMessageResourceName = nameof(CommonValidationMessages.SearchMatchTypeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public SearchMatchType SearchMatchType { get; set; } = SearchMatchType.None;
    }
}
