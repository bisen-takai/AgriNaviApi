namespace AgriNaviApi.Application.DTOs.ShippingDestinations
{
    /// <summary>
    /// 出荷先名検索レスポンス
    /// </summary>
    public class ShippingDestinationSearchDto
    {
        /// <summary>
        /// 検索結果の出荷先名一覧
        /// </summary>
        public IEnumerable<ShippingDestinationListItemDto> SearchItems { get; set; } = Enumerable.Empty<ShippingDestinationListItemDto>();

        /// <summary>
        /// 検索結果の件数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
