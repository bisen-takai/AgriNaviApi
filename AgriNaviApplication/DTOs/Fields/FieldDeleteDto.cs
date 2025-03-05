namespace AgriNaviApi.Application.DTOs.Fields
{
    /// <summary>
    /// 圃場削除レスポンス
    /// </summary>
    public class FieldDeleteDto
    {
        /// <summary>
        /// 圃場ID(自動インクリメントID)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 削除成功か
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 削除結果に関するメッセージ
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
