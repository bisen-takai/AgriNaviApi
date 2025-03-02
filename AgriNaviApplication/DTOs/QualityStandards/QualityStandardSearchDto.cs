namespace AgriNaviApi.Application.DTOs.QualityStandards
{
    /// <summary>
    /// 品質・規格検索レスポンス
    /// </summary>
    public class QualityStandardSearchDto
    {
        /// <summary>
        /// 検索結果の品質・規格一覧
        /// </summary>
        public IEnumerable<QualityStandardListItemDto> SearchItems { get; set; } = Enumerable.Empty<QualityStandardListItemDto>();

        /// <summary>
        /// 検索結果の件数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
