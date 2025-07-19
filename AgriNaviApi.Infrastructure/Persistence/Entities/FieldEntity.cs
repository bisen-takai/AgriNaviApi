using AgriNaviApi.Infrastructure.Interfaces;
using AgriNaviApi.Infrastructure.Persistence.Entities.Base;
using AgriNaviApi.Shared.ValidationRules;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 圃場情報テーブル
    /// </summary>
    [Table("fields")]
    [Index(nameof(GroupId), nameof(Name), IsUnique = true)]
    public class FieldEntity : BaseEntity
    {
        /// <summary>
        /// 圃場名
        /// </summary>
        [Column("field_name")]
        [Required]
        [MaxLength(FieldValidationRules.NameMax)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 圃場名省略名
        /// </summary>
        [Column("field_short_name")]
        [MaxLength(CommonValidationRules.ShortNameMax)]
        public string? ShortName { get; set; }

        /// <summary>
        /// 面積(m2)
        /// </summary>
        [Column("field_area_m2")]
        [Range(typeof(decimal), FieldValidationRules.AreaM2Min, FieldValidationRules.AreaM2Max)]
        public decimal? AreaM2 { get; set; }

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
        [MaxLength(CommonValidationRules.RemarkMax)]
        public string? Remark { get; set; }

        /// <summary>
        /// EF Coreマッピング用
        /// </summary>
        [Obsolete("このコンストラクタはEF Coreが内部的に使用します。アプリケーションコードでの使用は避けてください。", error: false)]
        public FieldEntity() { }

        /// <summary>
        /// 非null許容型の外部キーのエンティティの初期値を設定する
        /// </summary>
        /// <param name="group">グループエンティティ</param>
        /// <param name="color">カラーエンティティ</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FieldEntity(GroupEntity group, ColorEntity color)
        {
            Group = group ?? throw new ArgumentNullException(nameof(group));
            GroupId = group.Id;
            Color = color ?? throw new ArgumentNullException(nameof(color));
            ColorId = color.Id;
        }
    }
}
