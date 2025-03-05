namespace AgriNaviApi.Application.DTOs.ShippingDestinations
{
    /// <summary>
    /// 検索結果表示用に必要な情報だけを持つ出荷先名情報
    /// </summary>
    public class ShippingDestinationListItemDto
    {
        /// <summary>
        /// 出荷先名ID(自動インクリメントID)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 出荷先名UUID
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// 出荷先名
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
