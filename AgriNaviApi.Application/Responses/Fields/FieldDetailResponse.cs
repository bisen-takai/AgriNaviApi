namespace AgriNaviApi.Application.Responses.Fields
{
    /// <summary>
    /// 圃場詳細取得時のレスポンス
    /// </summary>
    public record FieldDetailResponse : FieldBaseResponse
    {
        /// <summary>
        /// 登録日時
        /// </summary>
        public DateTime CreatedAt { get; init; }

        /// <summary>
        /// 最終更新日時
        /// </summary>
        public DateTime LastUpdatedAt { get; init; }
    }
}
