using AgriNaviApi.Common.Enums;

namespace AgriNaviApi.Application.DTOs.Groups
{
    /// <summary>
    /// 検索結果表示用に必要な情報だけを持つグループ情報
    /// </summary>
    public class GroupListItemDto
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
    }
}
