using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 圃場情報テーブル
    /// </summary>
    [Table("fields")]
    public class FieldPersistenceEntity
    {
        /// <summary>
        /// 圃場情報ID(自動インクリメントID)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("field_id")]
        public int Id { get; set; }

        /// <summary>
        /// 圃場情報UUID
        /// </summary>
        [Column("field_uuid")]
        public Guid Uuid { get; set; }

        /// <summary>
        /// 圃場名
        /// </summary>
        [Column("field_name")]
        [Required]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 圃場名省略名
        /// </summary>
        [Column("field_short_name")]
        [Required]
        [StringLength(4)]
        public string? ShortName { get; set; }

        /// <summary>
        /// 面積(a)
        /// </summary>
        [Column("field_size")]
        public int FarmSize { get; set; }

        /// <summary>
        /// グループID
        /// </summary>
        [Column("group_id")]
        public int GroupId { get; set; }

        /// <summary>
        /// グループエンティティ
        /// </summary>
        [ForeignKey(nameof(GroupId))]
        public GroupPersistenceEntity Group { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        [Column("color_id")]
        public int ColorId { get; set; }

        /// <summary>
        /// カラーエンティティ
        /// </summary>
        [ForeignKey(nameof(ColorId))]
        public ColorPersistenceEntity Color { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("field_remark")]
        [StringLength(200)]
        public string? Remark { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("field_delete_flg")]
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

        /// <summary>
        /// 非null許容型の外部キーのエンティティの初期値がない場合はnullを設定する
        /// </summary>
        public FieldPersistenceEntity()
        {
            Group = null!;
            Color = null!;
        }

        /// <summary>
        /// 非null許容型の外部キーのエンティティの初期値を設定する
        /// </summary>
        /// <param name="group">グループエンティティ</param>
        /// <param name="color">カラーエンティティ</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FieldPersistenceEntity(GroupPersistenceEntity group, ColorPersistenceEntity color)
        {
            Group = group ?? throw new ArgumentNullException(nameof(group));
            Color = color ?? throw new ArgumentNullException(nameof(color));
        }
    }
}
