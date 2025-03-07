using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.SeasonCropSchedules
{
    /// <summary>
    /// 作付計画検索リクエスト
    /// </summary>
    public class SeasonCropScheduleSearchRequest
    {
        /// <summary>
        /// 検索作付計画名
        /// </summary>
        [Display(Name = "検索作付計画名")]
        public string? SearchSeasonCropScheduleName { get; set; }

        /// <summary>
        /// 検索一致タイプ
        /// </summary>
        [Display(Name = "検索一致タイプ")]
        [Range((int)SearchMatchType.None, (int)SearchMatchType.PARTIAL, ErrorMessageResourceName = nameof(CommonValidationMessages.SearchMatchTypeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public SearchMatchType SearchMatchType { get; set; } = SearchMatchType.None;

        /// <summary>
        /// 計画開始年月
        /// </summary>
        [Display(Name = "計画開始年月")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 計画終了年月
        /// </summary>
        [Display(Name = "計画終了年月")]
        public DateTime? EndDate { get; set; }
    }
}
