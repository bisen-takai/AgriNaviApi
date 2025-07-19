using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using AgriNaviApi.Infrastructure.Persistence.Entities.Base;
using AgriNaviApi.Shared.ValidationRules;
using AgriNaviApi.Infrastructure.Interfaces;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 単位名テーブル
    /// </summary>
    [Table("units")]
    [Index(nameof(Name), IsUnique = true)]
    public class UnitEntity : BaseEntity
    {
        /// <summary>
        /// 単位名
        /// </summary>
        [Column("unit_name")]
        [Required]
        [MaxLength(UnitValidationRules.NameMax)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// EF Coreマッピング用
        /// </summary>
        [Obsolete("このコンストラクタはEF Coreが内部的に使用します。アプリケーションコードでの使用は避けてください。", error: false)]
        public UnitEntity() { }
    }
}
