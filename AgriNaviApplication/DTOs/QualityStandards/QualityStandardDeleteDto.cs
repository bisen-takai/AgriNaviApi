namespace AgriNaviApi.Application.DTOs.QualityStandards
{
    /// <summary>
    /// 品質・規格削除レスポンス
    /// </summary>
    public class QualityStandardDeleteDto
    {
        /// <summary>
        /// 品質・規格ID(自動インクリメントID)
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
