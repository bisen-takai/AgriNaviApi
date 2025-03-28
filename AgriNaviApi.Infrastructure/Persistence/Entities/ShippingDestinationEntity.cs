using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 出荷先名テーブル
    /// </summary>
    [Table("shipping_destinations")]
    public class ShippingDestinationEntity
    {
        /// <summary>
        /// 出荷先名ID(自動インクリメントID)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("shipping_destination_id")]
        public int Id { get; set; }

        /// <summary>
        /// 出荷先名UUID
        /// </summary>
        [Column("shipping_destination_uuid")]
        public Guid Uuid { get; set; }

        /// <summary>
        /// 出荷先名
        /// </summary>
        [Column("shipping_destination_name")]
        [Required]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("shipping_destination_delete_flg")]
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
    }
}
