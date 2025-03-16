using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AgriNaviApi.Common.Enums;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// グループ名テーブル
    /// </summary>
    [Table("groups")]
    public class GroupEntity
    {
        /// <summary>
        /// グループ名ID(自動インクリメントID)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("group_id")]
        public int Id { get; set; }

        /// <summary>
        /// グループ名UUID
        /// </summary>
        [Column("group_uuid")]
        public Guid Uuid { get; set; }

        /// <summary>
        /// グループ名
        /// </summary>
        [Column("group_name")]
        [Required]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// グループ種別
        /// </summary>
        [Column("group_kind")]
        [EnumDataType(typeof(GroupKind))]
        public GroupKind Kind { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("group_delete_flg")]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最終更新日時
        /// </summary>
        [Column("last_updated_at")]
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
