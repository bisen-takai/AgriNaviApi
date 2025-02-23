namespace AgriNaviApi.Application.DTOs.Colors
{
    /// <summary>
    /// カラー検索レスポンス
    /// </summary>
    public class ColorSearchDto
    {
        /// <summary>
        /// 検索結果のカラー一覧
        /// </summary>
        public IEnumerable<ColorListItemDto> SearchItems { get; set; } = Enumerable.Empty<ColorListItemDto>();

        /// <summary>
        /// 検索結果の件数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
