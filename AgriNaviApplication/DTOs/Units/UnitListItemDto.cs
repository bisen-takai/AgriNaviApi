namespace AgriNaviApi.Application.DTOs.Units
{
    /// <summary>
    /// 検索結果表示用に必要な情報だけを持つ単位情報
    /// </summary>
    public class UnitListItemDto
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
    }
}
