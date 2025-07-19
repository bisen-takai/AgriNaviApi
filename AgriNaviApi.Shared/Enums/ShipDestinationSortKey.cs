namespace AgriNaviApi.Shared.Enums
{
    /// <summary>
    /// 出荷先のソートキー
    /// </summary>
    public enum ShipDestinationSortKey : byte
    {
        /// <summary>
        /// IDでソート
        /// </summary>
        Id = 0,
        /// <summary>
        /// 名称でソート
        /// </summary>
        Name = 1
    }
}
