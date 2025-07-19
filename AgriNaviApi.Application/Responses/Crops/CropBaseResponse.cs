namespace AgriNaviApi.Application.Responses.Crops
{
    /// <summary>
    /// 作付レスポンスの基底クラス
    /// </summary>
    public abstract record CropBaseResponse
    {
        /// <summary>
        /// 作付ID(自動インクリメントID)
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// 作付名
        /// </summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// 作付省略名
        /// </summary>
        public string? ShortName { get; init; }

        /// <summary>
        /// グループID
        /// </summary>
        public int GroupId { get; init; }

        /// <summary>
        /// グループ名
        /// </summary>
        public string GroupName { get; init; } = string.Empty;

        /// <summary>
        /// カラーID
        /// </summary>
        public int ColorId { get; init; }

        /// <summary>
        /// カラー情報
        /// </summary>
        public required ColorResponse Color { get; init; }

        /// <summary>
        /// 備考
        /// </summary>
        public string? Remark { get; init; }
    }
}
