namespace AgriNaviApi.Shared.Enums
{
    /// <summary>
    /// 作付のソートキー
    /// </summary>
    public enum CropSortKey : byte
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
        /// グループでソート
        /// </summary>
        Group = 2
    }
}
