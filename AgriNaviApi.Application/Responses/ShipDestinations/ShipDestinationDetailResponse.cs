namespace AgriNaviApi.Application.Responses.ShipDestinations
{
    /// <summary>
    /// 出荷先詳細取得時のレスポンス
    /// </summary>
    public record ShipDestinationDetailResponse : ShipDestinationBaseResponse
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
