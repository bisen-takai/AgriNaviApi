namespace AgriNaviApi.Application.Responses.QualityStandards
{
    /// <summary>
    /// 品質・規格レスポンスの基底クラス
    /// </summary>
    public abstract record QualityStandardBaseResponse
    {
        /// <summary>
        /// 品質・規格ID(自動インクリメントID)
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// 品質・規格名
        /// </summary>
        public string Name { get; init; } = string.Empty;
    }
}
