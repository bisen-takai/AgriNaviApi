using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using AgriNaviApi.Infrastructure.Persistence.Entities.Base;
using AgriNaviApi.Shared.ValidationRules;
using AgriNaviApi.Infrastructure.Interfaces;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// カラーテーブル
    /// </summary>
    [Table("colors")]
    [Index(nameof(Name), IsUnique = true)]
    public class ColorEntity : BaseEntity
    {
        /// <summary>
        /// カラー名
        /// </summary>
        [Column("color_name")]
        [Required]
        [MaxLength(ColorValidationRules.NameMax)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Red値
        /// </summary>
        [Column("color_red")]
        public byte Red { get; set; }

        /// <summary>
        /// Green値
        /// </summary>
        [Column("color_green")]
        public byte Green { get; set; }

        /// <summary>
        /// Blue値
        /// </summary>
        [Column("color_blue")]
        public byte Blue { get; set; }

        /// <summary>
        /// EF Coreマッピング用
        /// </summary>
        [Obsolete("このコンストラクタはEF Coreが内部的に使用します。アプリケーションコードでの使用は避けてください。", error: false)]
        public ColorEntity() { }
    }
}
