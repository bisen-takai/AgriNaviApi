namespace AgriNaviApi.Application.Responses.ShipmentLines
{
    /// <summary>
    /// 出荷記録詳細の詳細取得時のレスポンス
    /// </summary>
    public record ShipmentLineDetailResponse : ShipmentLineBaseResponse
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
