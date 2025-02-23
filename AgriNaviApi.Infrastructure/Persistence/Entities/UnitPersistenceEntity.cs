using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 単位名テーブル
    /// </summary>
    [Table("units")]
    public class UnitPersistenceEntity
    {
        /// <summary>
        /// 単位名ID(自動インクリメントID)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("unit_id")]
        public int Id { get; set; }

        /// <summary>
        /// 単位名UUID
        /// </summary>
        [Column("unit_uuid")]
        public Guid Uuid { get; set; }

        /// <summary>
        /// 単位名
        /// </summary>
        [Column("unit_name")]
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("unit_delete_flg")]
        public bool IsDeleted { get; set; }

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
