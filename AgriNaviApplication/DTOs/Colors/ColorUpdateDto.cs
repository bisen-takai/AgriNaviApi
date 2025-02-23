﻿namespace AgriNaviApi.Application.DTOs.Colors
{
    /// <summary>
    /// カラー更新レスポンス
    /// </summary>
    public class ColorUpdateDto
    {
        /// <summary>
        /// カラーID(自動インクリメントID)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// カラーUUID
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Red値
        /// </summary>
        public int RedValue { get; set; }

        /// <summary>
        /// Green値
        /// </summary>
        public int GreenValue { get; set; }

        /// <summary>
        /// Blue値
        /// </summary>
        public int BlueValue { get; set; }

        /// <summary>
        /// 最終更新日時
        /// </summary>
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
