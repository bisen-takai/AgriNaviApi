using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using AgriNaviApi.Infrastructure.Persistence.Entities.Base;
using AgriNaviApi.Shared.ValidationRules;
using AgriNaviApi.Infrastructure.Interfaces;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 作付テーブル
    /// </summary>
    [Table("crops")]
    [Index(nameof(GroupId), nameof(Name), IsUnique = true)]
    public class CropEntity : BaseEntity
    {
        /// <summary>
        /// 作付名
        /// </summary>
        [Column("crop_name")]
        [Required]
        [MaxLength(CropValidationRules.NameMax)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 作付名省略名
        /// </summary>
        [Column("crop_short_name")]
        [MaxLength(CommonValidationRules.ShortNameMax)]
        public string? ShortName { get; set; }

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
        [Column("crop_remark")]
        [MaxLength(CommonValidationRules.RemarkMax)]
        public string? Remark { get; set; }

        /// <summary>
        /// EF Coreマッピング用
        /// </summary>
        [Obsolete("このコンストラクタはEF Coreが内部的に使用します。アプリケーションコードでの使用は避けてください。", error: false)]
        public CropEntity() { }

        /// <summary>
        /// 非null許容型の外部キーのエンティティの初期値を設定する
        /// </summary>
        /// <param name="group">グループエンティティ</param>
        /// <param name="color">カラーエンティティ</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CropEntity(GroupEntity group, ColorEntity color)
        {
            Group = group ?? throw new ArgumentNullException(nameof(group));
            GroupId = group.Id;
            Color = color ?? throw new ArgumentNullException(nameof(color));
            ColorId = color.Id;
        }
    }
}
