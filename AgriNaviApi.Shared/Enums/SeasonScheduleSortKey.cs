namespace AgriNaviApi.Shared.Enums
{
    /// <summary>
    /// 作付計画のソートキー
    /// </summary>
    public enum SeasonScheduleSortKey : byte
    {
        /// <summary>
        /// IDでソート
        /// </summary>
        Id = 0,
        /// <summary>
        /// 名称でソート
        /// </summary>
        Name = 1,
        /// <summary>
        /// 作付名でソート
        /// </summary>
        Crop = 2,
        /// <summary>
        /// 計画開始日
        /// </summary>
        StartDate = 3,
        /// <summary>
        /// 計画終了日
        /// </summary>
        EndDate = 4
    }
}
