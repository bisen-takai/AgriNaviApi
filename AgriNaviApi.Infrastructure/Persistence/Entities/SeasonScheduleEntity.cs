using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using AgriNaviApi.Infrastructure.Persistence.Entities.Base;
using AgriNaviApi.Shared.ValidationRules;
using AgriNaviApi.Infrastructure.Interfaces;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 作付計画テーブル
    /// </summary>
    [Table("season_schedules")]
    [Index(nameof(Name), IsUnique = true)]
    public class SeasonScheduleEntity : BaseEntity, IHasUuid, ISoftDelete
    {
        /// <summary>
        /// 作付計画UUID（アプリ側から直接設定不可。SaveChanges内で自動設定）
        /// </summary>
        [Column("season_schedule_uuid")]
        public Guid Uuid { get; private set; }

        /// <summary>
        /// 作付計画名
        /// </summary>
        [Column("season_schedule_name")]
        [Required]
        [MaxLength(SeasonScheduleValidationRules.NameMax)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 作付ID
        /// </summary>
        [Column("crop_id")]
        public int CropId { get; set; }

        /// <summary>
        /// 作付名エンティティ
        /// </summary>
        [ForeignKey(nameof(CropId))]
        public CropEntity Crop { get; set; } = null!;

        /// <summary>
        /// 計画開始年月日
        /// </summary>
        [Column("season_schedule_start_date", TypeName = "date")]
        [Required]
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// 計画終了年月日
        /// </summary>
        [Column("season_schedule_end_date", TypeName = "date")]
        public DateOnly? EndDate { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Column("season_schedule_remark")]
        [MaxLength(CommonValidationRules.RemarkMax)]
        public string? Remark { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("delete_flg")]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 削除日時（UTC）
        /// </summary>
        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// EF Coreマッピング用
        /// </summary>
        [Obsolete("このコンストラクタはEF Coreが内部的に使用します。アプリケーションコードでの使用は避けてください。", error: false)]
        public SeasonScheduleEntity() { }

        /// <summary>
        /// 非null許容型の外部キーのエンティティの初期値を設定する
        /// </summary>
        /// <param name="crop">作付名エンティティ</param>
        /// <exception cref="ArgumentNullException"></exception>
        public SeasonScheduleEntity(CropEntity crop)
        {
            Crop = crop ?? throw new ArgumentNullException(nameof(crop));
            CropId = crop.Id;
        }
    }
}
