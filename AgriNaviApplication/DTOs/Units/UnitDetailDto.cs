namespace AgriNaviApi.Application.DTOs.Units
{
    /// <summary>
    /// 単位詳細レスポンス
    /// </summary>
    public class UnitDetailDto
    {
        /// <summary>
        /// 単位ID(自動インクリメントID)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 単位UUID
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// 単位名
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
