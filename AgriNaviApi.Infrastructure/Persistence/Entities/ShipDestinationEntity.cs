using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using AgriNaviApi.Infrastructure.Persistence.Entities.Base;
using AgriNaviApi.Shared.ValidationRules;
using AgriNaviApi.Infrastructure.Interfaces;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 出荷先名テーブル
    /// </summary>
    [Table("ship_destinations")]
    [Index(nameof(Name), IsUnique = true)]
    public class ShipDestinationEntity : BaseEntity
    {
        /// <summary>
        /// 出荷先名
        /// </summary>
        [Column("shipping_destination_name")]
        [Required]
        [MaxLength(ShipDestinationValidationRules.NameMax)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// EF Coreマッピング用
        /// </summary>
        [Obsolete("このコンストラクタはEF Coreが内部的に使用します。アプリケーションコードでの使用は避けてください。", error: false)]
        public ShipDestinationEntity() { }
    }
}
