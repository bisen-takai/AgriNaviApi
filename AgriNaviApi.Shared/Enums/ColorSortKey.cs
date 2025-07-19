namespace AgriNaviApi.Shared.Enums
{
    /// <summary>
    /// カラーのソートキー
    /// </summary>
    public enum ColorSortKey : byte
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
        /// Red値でソート
        /// </summary>
        RedValue = 2,
        /// <summary>
        /// Green値でソート
        /// </summary>
        GreenValue = 3,
        /// <summary>
        /// Blue値でソート
        /// </summary>
        BlueValue = 4
    }
}
