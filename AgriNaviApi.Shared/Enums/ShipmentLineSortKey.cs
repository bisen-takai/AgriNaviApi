namespace AgriNaviApi.Shared.Enums
{
    /// <summary>
    /// 出荷記録詳細のソートキー
    /// </summary>
    public enum ShipmentLineSortKey : byte
    {
        /// <summary>
        /// 出荷記録でソート
        /// </summary>
        Shipment = 0,
        /// <summary>
        /// 出荷先名でソート
        /// </summary>
        ShipDestinationName = 1,
        /// <summary>
        /// 品質・規格名でソート
        /// </summary>
        QualityStandardName = 2
    }
}
