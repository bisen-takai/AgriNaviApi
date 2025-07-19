using AgriNaviApi.Infrastructure.Interfaces;
using AgriNaviApi.Infrastructure.Persistence.Entities.Base;
using AgriNaviApi.Shared.ValidationRules;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 品質・規格テーブル
    /// </summary>
    [Table("quality_standards")]
    [Index(nameof(Name), IsUnique = true)]
    public class QualityStandardEntity : BaseEntity
    {
        /// <summary>
        /// 品質・規格名
        /// </summary>
        [Column("quality_standard_name")]
        [Required]
        [MaxLength(QualityStandardValidationRules.NameMax)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// EF Coreマッピング用
        /// </summary>
        [Obsolete("このコンストラクタはEF Coreが内部的に使用します。アプリケーションコードでの使用は避けてください。", error: false)]
        public QualityStandardEntity() { }
    }
}
