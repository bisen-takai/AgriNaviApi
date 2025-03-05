using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.ShippingDestinations
{
    /// <summary>
    /// 出荷先名検索リクエスト
    /// </summary>
    public class ShippingDestinationSearchRequest
    {
        /// <summary>
        /// 検索出荷先名
        /// </summary>
        [Display(Name = "検索出荷先名")]
        public string? SearchShippingDestinationName { get; set; }

        /// <summary>
        /// 検索一致タイプ
        /// </summary>
        [Display(Name = "検索一致タイプ")]
        [Range((int)SearchMatchType.None, (int)SearchMatchType.PARTIAL, ErrorMessageResourceName = nameof(CommonValidationMessages.SearchMatchTypeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public SearchMatchType SearchMatchType { get; set; } = SearchMatchType.None;
    }
}
