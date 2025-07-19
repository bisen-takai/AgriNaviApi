namespace AgriNaviApi.Application.Responses.Fields
{
    /// <summary>
    /// 圃場レスポンスの基底クラス
    /// </summary>
    public abstract record FieldBaseResponse
    {
        /// <summary>
        /// 圃場ID(自動インクリメントID)
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// 圃場名
        /// </summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// 圃場省略名
        /// </summary>
        public string? ShortName { get; init; }

        /// <summary>
        /// 面積(m2)
        /// </summary>
        public decimal FieldSize { get; init; }

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
