using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 出荷記録テーブル
    /// </summary>
    [Table("shipment_records")]
    public class ShipmentRecordEntity
    {
        /// <summary>
        /// 出荷記録ID(自動インクリメントID)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("shipment_record_id")]
        public int Id { get; set; }

        /// <summary>
        /// 出荷記録UUID
        /// </summary>
        [Column("shipment_record_uuid")]
        public Guid Uuid { get; set; }

        /// <summary>
        /// 日付
        /// </summary>
        [Column("shipment_record_date", TypeName = "date")]
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// 圃場ID
        /// </summary>
        [Column("field_id")]
        public int FieldId { get; set; }

        /// <summary>
        /// 圃場エンティティ
        /// </summary>
        [ForeignKey(nameof(FieldId))]
        public FieldEntity? Field { get; set; } = null!;

        /// <summary>
        /// 作付ID
        /// </summary>
        [Column("crop_id")]
        public int CropId { get; set; }

        /// <summary>
        /// 作付エンティティ
        /// </summary>
        [ForeignKey(nameof(CropId))]
        public CropEntity Crop { get; set; } = null!;

        /// <summary>
        /// 作付年間計画ID
        /// </summary>
        [Column("season_crop_schedule_id")]
        public int SeasonCropScheduleId { get; set; }

        /// <summary>
        /// 作付年間計画エンティティ
        /// </summary>
        [ForeignKey(nameof(SeasonCropScheduleId))]
        public SeasonCropScheduleEntity SeasonCropSchedule { get; set; } = null!;

        /// <summary>
        /// 備考
        /// </summary>
        [Column("shipment_record_remark")]
        [StringLength(200)]
        public string? Remark { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("shipment_record_delete_flg")]
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
        /// 出荷詳細エンティティのコレクション
        /// </summary>
        public ICollection<ShipmentRecordDetailEntity>? Details { get; set; }

        /// <summary>
        /// EF Coreマッピング用
        /// </summary>
        public ShipmentRecordEntity()
        {
        }

        /// <summary>
        /// 非null許容型の外部キーのエンティティの初期値を設定する
        /// </summary>
        /// <param name="crop">作付名エンティティ</param>
        /// <param name="seasonCropSchedule">作付計画エンティティ</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ShipmentRecordEntity(CropEntity crop, SeasonCropScheduleEntity seasonCropSchedule)
        {
            Crop = crop ?? throw new ArgumentNullException(nameof(crop));
            SeasonCropSchedule = seasonCropSchedule ?? throw new ArgumentNullException(nameof(seasonCropSchedule));
        }
    }
}
