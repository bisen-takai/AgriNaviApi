namespace AgriNaviApi.Application.DTOs.Fields
{
    /// <summary>
    /// 圃場登録レスポンス
    /// </summary>
    public class FieldCreateDto
    {
        /// <summary>
        /// 圃場ID(自動インクリメントID)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 圃場UUID
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// 圃場名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 圃場省略名
        /// </summary>
        public string? ShortenName { get; set; }

        /// <summary>
        /// 面積(a)
        /// </summary>
        public int FieldSize { get; set; }

        /// <summary>
        /// グループID
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// グループ名
        /// </summary>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        /// カラーID
        /// </summary>
        public int ColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        public string ColorName { get; set; } = string.Empty;

        /// <summary>
        /// Red値
        /// </summary>
        public int ColorRedValue { get; set; }

        /// <summary>
        /// Green値
        /// </summary>
        public int ColorGreenValue { get; set; }

        /// <summary>
        /// Blue値
        /// </summary>
        public int ColorBlueValue { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        public string? Remark { get; set; }

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
