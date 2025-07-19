namespace AgriNaviApi.Application.Responses.Users
{
    /// <summary>
    /// ユーザ詳細取得時のレスポンス
    /// </summary>
    public record UserDetailResponse : UserBaseResponse
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
