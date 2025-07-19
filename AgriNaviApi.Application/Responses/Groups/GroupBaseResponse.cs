using AgriNaviApi.Shared.Enums;

namespace AgriNaviApi.Application.Responses.Groups
{
    /// <summary>
    /// グループレスポンスの基底クラス
    /// </summary>
    public abstract record GroupBaseResponse
    {
        /// <summary>
        /// グループID(自動インクリメントID)
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// グループ名
        /// </summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// グループ種別
        /// </summary>
        public GroupKind Kind { get; init; }
    }
}
