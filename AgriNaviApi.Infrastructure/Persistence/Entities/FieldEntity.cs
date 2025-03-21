﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 圃場情報テーブル
    /// </summary>
    [Table("fields")]
    public class FieldEntity
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
        [StringLength(4)]
        public string? ShortName { get; set; }

        /// <summary>
        /// 面積(m2)
        /// </summary>
        [Column("field_size")]
        [Range(0, 100000)]
        public int FieldSize { get; set; }

        /// <summary>
        /// グループID
        /// </summary>
        [Column("group_id")]
        public int GroupId { get; set; }

        /// <summary>
        /// グループエンティティ
        /// </summary>
        [ForeignKey(nameof(GroupId))]
        public GroupEntity Group { get; set; } = null!;

        /// <summary>
        /// カラーID
        /// </summary>
        [Column("color_id")]
        public int ColorId { get; set; }

        /// <summary>
        /// カラーエンティティ
        /// </summary>
        [ForeignKey(nameof(ColorId))]
        public ColorEntity Color { get; set; } = null!;

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

        /// <summary>
        /// EF Coreマッピング用
        /// </summary>
        public FieldEntity()
        {
        }

        /// <summary>
        /// 非null許容型の外部キーのエンティティの初期値を設定する
        /// </summary>
        /// <param name="group">グループエンティティ</param>
        /// <param name="color">カラーエンティティ</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FieldEntity(GroupEntity group, ColorEntity color)
        {
            Group = group ?? throw new ArgumentNullException(nameof(group));
            Color = color ?? throw new ArgumentNullException(nameof(color));
        }
    }
}
