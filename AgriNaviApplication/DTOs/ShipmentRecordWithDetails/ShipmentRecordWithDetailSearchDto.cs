namespace AgriNaviApi.Application.DTOs.ShipmentRecords
{
    /// <summary>
    /// 出荷記録検索レスポンス
    /// </summary>
    public class ShipmentRecordWithDetailSearchDto
    {
        /// 検索結果の出荷記録一覧
        /// </summary>
        public IEnumerable<ShipmentRecordWithDetailListItemDto> SearchItems { get; set; } = Enumerable.Empty<ShipmentRecordWithDetailListItemDto>();

        /// <summary>
        /// 検索結果の件数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
