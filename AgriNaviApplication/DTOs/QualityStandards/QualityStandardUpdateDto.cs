namespace AgriNaviApi.Application.DTOs.QualityStandards
{
    /// <summary>
    /// 品質・規格更新レスポンス
    /// </summary>
    public class QualityStandardUpdateDto
    {
        /// <summary>
        /// 品質・規格ID(自動インクリメントID)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 品質・規格UUID
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// 品質・規格名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 最終更新日時
        /// </summary>
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
