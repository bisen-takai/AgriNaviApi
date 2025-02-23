namespace AgriNaviApi.Common.Enums
{
    /// <summary>
    /// 検索一致タイプ
    /// </summary>
    public enum SearchMatchType
    {
        /// <summary>
        /// 指定なし
        /// </summary>
        None = 0,

        /// <summary>
        /// 完全一致
        /// </summary>
        EXACT = 1,

        /// <summary>
        /// 前方一致
        /// </summary>
        PREFIX = 2,

        /// <summary>
        /// 後方一致
        /// </summary>
        SUFFIX = 3,

        /// <summary>
        /// 部分一致
        /// </summary>
        PARTIAL = 4
    }
}
