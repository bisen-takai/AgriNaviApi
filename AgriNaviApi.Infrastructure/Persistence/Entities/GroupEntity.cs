using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AgriNaviApi.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using AgriNaviApi.Infrastructure.Persistence.Entities.Base;
using AgriNaviApi.Shared.ValidationRules;
using AgriNaviApi.Infrastructure.Interfaces;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// グループテーブル
    /// </summary>
    [Table("groups_mst")]
    [Index(nameof(Name), IsUnique = true)]
    public class GroupEntity : BaseEntity
    {
        /// <summary>
        /// グループ名
        /// </summary>
        [Column("group_name")]
        [Required]
        [MaxLength(GroupValidationRules.NameMax)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// グループ種別
        /// </summary>
        [Column("group_kind")]
        [EnumDataType(typeof(GroupKind))]
        public GroupKind Kind { get; set; }

        /// <summary>
        /// EF Coreマッピング用
        /// </summary>
        [Obsolete("このコンストラクタはEF Coreが内部的に使用します。アプリケーションコードでの使用は避けてください。", error: false)]
        public GroupEntity() { }
    }
}
