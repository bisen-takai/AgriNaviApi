namespace AgriNaviApi.Application.DTOs.ShippingDestinations
{
    /// <summary>
    /// 出荷先登録レスポンス
    /// </summary>
    public class ShippingDestinationCreateDto
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

        /// <summary>
        /// 登録日時
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最終更新日時
        /// </summary>
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
