namespace AgriNaviApi.Application.DTOs.Units
{
    /// <summary>
    /// 単位削除レスポンス
    /// </summary>
    public class UnitDeleteDto
    {
        /// <summary>
        /// 単位ID(自動インクリメントID)
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
