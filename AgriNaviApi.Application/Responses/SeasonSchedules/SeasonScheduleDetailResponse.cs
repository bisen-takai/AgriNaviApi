namespace AgriNaviApi.Application.Responses.SeasonSchedules
{
    /// <summary>
    /// 作付計画詳細取得時のレスポンス
    /// </summary>
    public record SeasonScheduleDetailResponse : SeasonScheduleBaseResponse
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
