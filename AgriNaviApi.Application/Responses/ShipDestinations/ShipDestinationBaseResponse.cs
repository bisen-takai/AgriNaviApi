namespace AgriNaviApi.Application.Responses.ShipDestinations
{
    /// <summary>
    /// 出荷先レスポンスの基底クラス
    /// </summary>
    public abstract record ShipDestinationBaseResponse
    {
        /// <summary>
        /// 出荷先ID(自動インクリメントID)
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// 出荷先名
        /// </summary>
        public string Name { get; init; } = string.Empty;
    }
}
