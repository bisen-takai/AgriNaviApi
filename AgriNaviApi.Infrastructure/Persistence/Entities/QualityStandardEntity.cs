using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 品質・規格名テーブル
    /// </summary>
    [Table("quality_standards")]
    public class QualityStandardEntity
    {
        /// <summary>
        /// 品質・規格名ID(自動インクリメントID)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("quality_standard_id")]
        public int Id { get; set; }

        /// <summary>
        /// 品質・規格名UUID
        /// </summary>
        [Column("quality_standard_uuid")]
        public Guid Uuid { get; set; }

        /// <summary>
        /// 品質・規格名
        /// </summary>
        [Column("quality_standard_name")]
        [Required]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("quality_standard_delete_flg")]
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
