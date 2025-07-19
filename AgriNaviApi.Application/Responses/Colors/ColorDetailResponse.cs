namespace AgriNaviApi.Application.Responses.Colors
{
    /// <summary>
    /// カラー詳細取得時のレスポンス
    /// </summary>
    public record ColorDetailResponse : ColorBaseResponse
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
