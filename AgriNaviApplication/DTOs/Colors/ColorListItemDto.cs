namespace AgriNaviApi.Application.DTOs.Colors
{
    /// <summary>
    /// 検索結果表示用に必要な情報だけを持つカラー情報
    /// </summary>
    public class ColorListItemDto
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
    }
}
