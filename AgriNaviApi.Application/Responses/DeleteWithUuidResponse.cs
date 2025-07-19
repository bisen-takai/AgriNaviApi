namespace AgriNaviApi.Application.Responses
{
    /// <summary>
    /// 削除時のレスポンス
    /// </summary>
    public class DeleteWithUuidResponse
    {
        /// <summary>
        /// UUID
        /// </summary>
        public Guid Uuid { get; set; }

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
