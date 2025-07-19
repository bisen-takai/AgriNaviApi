namespace AgriNaviApi.Application.Responses.Units
{
    /// <summary>
    /// 単位詳細取得時のレスポンス
    /// </summary>
    public record UnitDetailResponse : UnitBaseResponse
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
