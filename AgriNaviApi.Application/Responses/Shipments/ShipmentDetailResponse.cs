namespace AgriNaviApi.Application.Responses.Shipments
{
    /// <summary>
    /// 出荷記録詳細取得時のレスポンス
    /// </summary>
    public record ShipmentDetailResponse : ShipmentBaseResponse
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
