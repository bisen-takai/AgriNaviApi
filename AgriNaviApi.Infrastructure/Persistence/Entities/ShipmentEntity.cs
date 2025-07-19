using AgriNaviApi.Infrastructure.Interfaces;
using AgriNaviApi.Infrastructure.Persistence.Entities.Base;
using AgriNaviApi.Shared.ValidationRules;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 出荷記録テーブル
    /// </summary>
    [Table("shipments")]
    [Index(nameof(ShipmentDate), nameof(SeasonScheduleId), IsUnique = true)]
    public class ShipmentEntity : BaseEntity, IHasUuid, ISoftDelete
    {
        /// <summary>
        /// 出荷記録UUID（アプリ側から直接設定不可。SaveChanges内で自動設定）
        /// </summary>
        /// <remarks>
        /// 基本的にアプリケーションコードではUUIDを指定しない。Guid.Emptyで追加され、SaveChangesAsync() 内で自動生成される。
        /// </remarks>
        [Column("shipment_uuid")]
        public Guid Uuid { get; private set; }

        /// <summary>
        /// 出荷日付
        /// </summary>
        [Column("shipment_date", TypeName = "date")]
        public DateOnly ShipmentDate { get; set; }

        /// <summary>
        /// 圃場ID
        /// </summary>
        [Column("field_id")]
        public int FieldId { get; set; }

        /// <summary>
        /// 圃場エンティティ
        /// </summary>
        [ForeignKey(nameof(FieldId))]
        public FieldEntity Field { get; set; } = null!;

        /// <summary>
        /// 作付年間計画ID
        /// </summary>
        [Column("season_schedule_id")]
        public int SeasonScheduleId { get; set; }

        /// <summary>
        /// 作付年間計画エンティティ
        /// </summary>
        [ForeignKey(nameof(SeasonScheduleId))]
        public SeasonScheduleEntity SeasonSchedule { get; set; } = null!;

        /// <summary>
        /// 備考
        /// </summary>
        [Column("shipment_remark")]
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
        /// 出荷詳細エンティティのコレクション
        /// </summary>
        [InverseProperty(nameof(ShipmentLineEntity.Shipment))]
        public ICollection<ShipmentLineEntity>? Lines { get; set; } = new List<ShipmentLineEntity>();

        /// <summary>
        /// EF Coreマッピング用
        /// </summary>
        [Obsolete("このコンストラクタはEF Coreが内部的に使用します。アプリケーションコードでの使用は避けてください。", error: false)]
        public ShipmentEntity() { }

        /// <summary>
        /// 非null許容型の外部キーのエンティティの初期値を設定する
        /// </summary>
        /// <param name="field">圃場エンティティ</param>
        /// <param name="seasonSchedule">作付計画エンティティ</param>
        /// <param name="uuid">出荷記録UUID</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ShipmentEntity(FieldEntity field, SeasonScheduleEntity seasonSchedule, Guid uuid)
        {
            Field = field ?? throw new ArgumentNullException(nameof(field));
            FieldId = field.Id;
            SeasonSchedule = seasonSchedule ?? throw new ArgumentNullException(nameof(seasonSchedule));
            SeasonScheduleId = seasonSchedule.Id;
            Uuid = uuid;
        }
    }
}
