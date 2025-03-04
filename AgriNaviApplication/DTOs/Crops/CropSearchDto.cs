namespace AgriNaviApi.Application.DTOs.Crops
{
    /// <summary>
    /// 作付名検索レスポンス
    /// </summary>
    public class CropSearchDto
    {
        /// <summary>
        /// 検索結果の作付名一覧
        /// </summary>
        public IEnumerable<CropListItemDto> SearchItems { get; set; } = Enumerable.Empty<CropListItemDto>();

        /// <summary>
        /// 検索結果の件数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
