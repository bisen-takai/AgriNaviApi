using AgriNaviApi.Common.Enums;

namespace AgriNaviApi.Application.DTOs.Groups
{
    /// <summary>
    /// グループ更新レスポンス
    /// </summary>
    public class GroupUpdateDto
    {
        /// <summary>
        /// グループID(自動インクリメントID)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// グループUUID
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// グループ名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// グループ種別
        /// </summary>
        public GroupKind Kind { get; set; }

        /// <summary>
        /// 最終更新日時
        /// </summary>
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
