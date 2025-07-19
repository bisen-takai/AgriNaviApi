namespace AgriNaviApi.Shared.Enums
{
    /// <summary>
    /// 出荷記録のソートキー
    /// </summary>
    public enum ShipmentSortKey : byte
    {
        /// <summary>
        /// 日付でソート
        /// </summary>
        Date = 0,
        /// <summary>
        /// 圃場名でソート
        /// </summary>
        Field = 1,
        /// <summary>
        /// 作付計画でソート
        /// </summary>
        SeasonSchedule = 2
    }
}
