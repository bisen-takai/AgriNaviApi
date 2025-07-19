namespace AgriNaviApi.Application.Responses.Units
{
    /// <summary>
    /// 単位レスポンスの基底クラス
    /// </summary>
    public abstract record UnitBaseResponse
    {
        /// <summary>
        /// 単位ID(自動インクリメントID)
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// 単位名
        /// </summary>
        public string Name { get; init; } = string.Empty;
    }
}
