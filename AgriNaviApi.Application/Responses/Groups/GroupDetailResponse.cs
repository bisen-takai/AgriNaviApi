namespace AgriNaviApi.Application.Responses.Groups
{
    /// <summary>
    /// グループ詳細取得時のレスポンス
    /// </summary>
    public record GroupDetailResponse : GroupBaseResponse
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
