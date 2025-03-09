namespace AgriNaviApi.Application.DTOs.ShipmentRecords
{
    /// <summary>
    /// 出荷記録検索レスポンス
    /// </summary>
    public class ShipmentRecordSearchDto
    {
        /// 検索結果の出荷記録一覧
        /// </summary>
        public IEnumerable<ShipmentRecordListItemDto> SearchItems { get; set; } = Enumerable.Empty<ShipmentRecordListItemDto>();

        /// <summary>
        /// 検索結果の件数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
