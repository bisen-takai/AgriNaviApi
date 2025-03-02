namespace AgriNaviApi.Application.DTOs.Units
{
    /// <summary>
    /// 単位検索レスポンス
    /// </summary>
    public class UnitSearchDto
    {
        /// <summary>
        /// 検索結果の単位一覧
        /// </summary>
        public IEnumerable<UnitListItemDto> SearchItems { get; set; } = Enumerable.Empty<UnitListItemDto>();

        /// <summary>
        /// 検索結果の件数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
