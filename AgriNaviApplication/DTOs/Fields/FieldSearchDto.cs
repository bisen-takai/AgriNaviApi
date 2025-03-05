namespace AgriNaviApi.Application.DTOs.Fields
{
    /// <summary>
    /// 圃場検索レスポンス
    /// </summary>
    public class FieldSearchDto
    {
        /// <summary>
        /// 検索結果の圃場一覧
        /// </summary>
        public IEnumerable<FieldListItemDto> SearchItems { get; set; } = Enumerable.Empty<FieldListItemDto>();

        /// <summary>
        /// 検索結果の件数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
