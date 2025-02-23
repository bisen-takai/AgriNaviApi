using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 作付計画テーブル
    /// </summary>
    [Table("season_crop_schedules")]
    public class SeasonCropScheduleEntity
    {
        /// <summary>
        /// 作付計画ID(自動インクリメントID)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("season_crop_schedule_id")]
        public int Id { get; set; }

        /// <summary>
        /// 作付計画UUID
        /// </summary>
        [Column("season_crop_schedule_uuid")]
        public Guid Uuid { get; set; }

        /// <summary>
        /// 作付計画名
        /// </summary>
        [Column("season_crop_schedule_name")]
        [Required]
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 作付名ID
        /// </summary>
        [Column("crop_id")]
        public int CropId { get; set; }

        /// <summary>
        /// 作付名エンティティ
        /// </summary>
        [ForeignKey(nameof(CropId))]
        public CropEntity Crop { get; set; } = null!;

        /// <summary>
        /// 計画開始年月
        /// </summary>
        [Column("season_crop_schedule_start_date", TypeName = "date")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 計画終了年月
        /// </summary>
        [Column("season_crop_schedule_end_date", TypeName = "date")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("season_crop_schedule_remark")]
        [StringLength(200)]
        public string? Remark { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("season_crop_schedule_delete_flg")]
        public bool IsDeleted { get; set; }

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
        public SeasonCropScheduleEntity()
        {
        }

        /// <summary>
        /// 非null許容型の外部キーのエンティティの初期値を設定する
        /// </summary>
        /// <param name="crop">作付名エンティティ</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SeasonCropScheduleEntity(CropEntity crop)
        {
            Crop = crop ?? throw new ArgumentNullException(nameof(crop));
        }
    }
}
