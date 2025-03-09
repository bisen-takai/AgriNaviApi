namespace AgriNaviApi.Application.DTOs.ShipmentRecordDetails
{
    /// <summary>
    /// 出荷記録詳細検索レスポンス
    /// </summary>
    public class ShipmentRecordDetailSearchDto
    {
        /// 検索結果の出荷記録一覧
        /// </summary>
        public IEnumerable<ShipmentRecordDetailListItemDto> SearchItems { get; set; } = Enumerable.Empty<ShipmentRecordDetailListItemDto>();

        /// <summary>
        /// 検索結果の件数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
