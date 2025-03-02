namespace AgriNaviApi.Application.DTOs.QualityStandards
{
    /// <summary>
    /// 検索結果表示用に必要な情報だけを持つ品質・規格情報
    /// </summary>
    public class QualityStandardListItemDto
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
    }
}
