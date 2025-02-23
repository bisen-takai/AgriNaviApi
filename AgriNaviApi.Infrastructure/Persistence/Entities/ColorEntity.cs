using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// Colorsテーブル
    /// </summary>
    [Table("colors")]
    public class ColorEntity
    {
        /// <summary>
        /// カラーID(自動インクリメントID)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("color_id")]
        public int Id { get; set; }

        /// <summary>
        /// カラーUUID
        /// </summary>
        [Column("color_uuid")]
        public Guid Uuid { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        [Column("color_name")]
        [Required]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Red値
        /// </summary>
        [Column("color_red_value")]
        [Range(0, 255)]
        public int RedValue { get; set; }

        /// <summary>
        /// Green値
        /// </summary>
        [Column("color_green_value")]
        [Range(0, 255)]
        public int GreenValue { get; set; }

        /// <summary>
        /// Blue値
        /// </summary>
        [Column("color_blue_value")]
        [Range(0, 255)]
        public int BlueValue { get; set; }

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
