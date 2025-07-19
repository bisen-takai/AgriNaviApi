namespace AgriNaviApi.Shared.Enums
{
    /// <summary>
    /// 圃場のソートキー
    /// </summary>
    public enum FieldSortKey : byte
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
