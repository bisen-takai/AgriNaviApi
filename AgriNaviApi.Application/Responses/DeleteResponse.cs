namespace AgriNaviApi.Application.Responses
{
    /// <summary>
    /// 削除時のレスポンス
    /// </summary>
    public class DeleteResponse
    {
        /// <summary>
        /// ID(自動インクリメントID)
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

        /// <summary>
        /// 削除日時
        /// </summary>
        public DateTime? DeletedAt { get; set; }
    }
}
