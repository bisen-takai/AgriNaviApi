namespace AgriNaviApi.Application.Responses.Colors
{
    /// <summary>
    /// カラーレスポンスの基底クラス
    /// </summary>
    public abstract record ColorBaseResponse
    {
        /// <summary>
        /// カラーID(自動インクリメントID)
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// カラー名
        /// </summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// Red値
        /// </summary>
        public int RedValue { get; init; }

        /// <summary>
        /// Green値
        /// </summary>
        public int GreenValue { get; init; }

        /// <summary>
        /// Blue値
        /// </summary>
        public int BlueValue { get; init; }
    }
}
