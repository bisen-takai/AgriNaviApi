namespace AgriNaviApi.Application.DTOs.SeasonCropSchedules
{
    /// <summary>
    /// 作付計画検索レスポンス
    /// </summary>
    public class SeasonCropScheduleSearchDto
    {
        /// <summary>
        /// 検索結果の作付計画一覧
        /// </summary>
        public IEnumerable<SeasonCropScheduleListItemDto> SearchItems { get; set; } = Enumerable.Empty<SeasonCropScheduleListItemDto>();

        /// <summary>
        /// 検索結果の件数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
