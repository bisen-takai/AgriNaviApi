namespace AgriNaviApi.Application.Responses.ShipmentLines
{
    /// <summary>
    /// 出荷記録詳細レスポンスの基底クラス
    /// </summary>
    public abstract record ShipmentLineBaseResponse
    {
        /// <summary>
        /// 出荷記録詳細UUID
        /// </summary>
        public Guid Uuid { get; init; }

        /// <summary>
        /// 出荷記録ID
        /// </summary>
        public int ShipmentId { get; init; }

        /// <summary>
        /// 出荷先ID
        /// </summary>
        public int ShipDestinationId { get; init; }

        /// <summary>
        /// 出荷先名
        /// </summary>
        public string? ShipDestinationName { get; init; }

        /// <summary>
        /// 品質規格ID
        /// </summary>
        public int QualityStandardId { get; init; }

        /// <summary>
        /// 品質規格名
        /// </summary>
        public string? QualityStandardName { get; init; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; init; }

        /// <summary>
        /// 単位ID
        /// </summary>
        public int UnitId { get; init; }

        /// <summary>
        /// 単位名
        /// </summary>
        public string? UnitName { get; init; }

        /// <summary>
        /// 金額
        /// </summary>
        public int Amount { get; init; }
    }
}
