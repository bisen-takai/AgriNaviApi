using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Crops
{
    /// <summary>
    /// 作付名検索リクエスト
    /// </summary>
    public class CropSearchRequest
    {
        /// <summary>
        /// 検索作付名
        /// </summary>
        [Display(Name = "検索作付名")]
        public string? SearchCropName { get; set; }

        /// <summary>
        /// 検索一致タイプ
        /// </summary>
        [Display(Name = "検索一致タイプ")]
        [Range((int)SearchMatchType.None, (int)SearchMatchType.PARTIAL, ErrorMessageResourceName = nameof(CommonValidationMessages.SearchMatchTypeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public SearchMatchType SearchMatchType { get; set; } = SearchMatchType.None;
    }
}
