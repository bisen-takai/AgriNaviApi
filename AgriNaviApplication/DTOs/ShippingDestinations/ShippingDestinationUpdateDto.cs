﻿namespace AgriNaviApi.Application.DTOs.ShippingDestinations
{
    /// <summary>
    /// 出荷先名更新レスポンス
    /// </summary>
    public class ShippingDestinationUpdateDto
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
        /// 最終更新日時
        /// </summary>
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
