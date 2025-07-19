using AgriNaviApi.Infrastructure.Interfaces;
using AgriNaviApi.Infrastructure.Persistence.Entities.Base;
using AgriNaviApi.Shared.ValidationRules;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 出荷記録詳細テーブル
    /// </summary>
    [Table("shipment_lines")]
    [Index(nameof(ShipmentId))]
    public class ShipmentLineEntity : BaseEntity, IHasUuid, ISoftDelete
    {
        /// <summary>
        /// 出荷記録詳細UUID（アプリ側から直接設定不可。SaveChanges内で自動設定）
        /// </summary>
        [Column("shipment_line_uuid")]
        public Guid Uuid { get; private set; }

        /// <summary>
        /// 出荷記録ID
        /// </summary>
        [Column("shipment_id")]
        public int ShipmentId { get; set; }

        /// <summary>
        /// 出荷記録エンティティ
        /// </summary>
        [ForeignKey(nameof(ShipmentId))]
        public ShipmentEntity Shipment { get; set; } = null!;

        /// <summary>
        /// 出荷先ID
        /// </summary>
        [Column("ship_destination_id")]
        public int ShipDestinationId { get; set; }

        /// <summary>
        /// 出荷先エンティティ
        /// </summary>
        [ForeignKey(nameof(ShipDestinationId))]
        public ShipDestinationEntity ShipDestination { get; set; } = null!;

        /// <summary>
        /// 品質規格ID
        /// </summary>
        [Column("quality_standard_id")]
        public int QualityStandardId { get; set; }

        /// <summary>
        /// 品質規格エンティティ
        /// </summary>
        [ForeignKey(nameof(QualityStandardId))]
        public QualityStandardEntity QualityStandard { get; set; } = null!;

        /// <summary>
        /// 数量
        /// </summary>
        [Column("shipment_line_quantity")]
        [Range(ShipmentLineValidationRules.QuantityMin, ShipmentLineValidationRules.QuantityMax)]
        public int Quantity { get; set; }

        /// <summary>
        /// 単位ID
        /// </summary>
        [Column("unit_id")]
        public int UnitId { get; set; }

        /// <summary>
        /// 単位エンティティ
        /// </summary>
        [ForeignKey(nameof(UnitId))]
        public UnitEntity Unit { get; set; } = null!;

        /// <summary>
        /// 金額
        /// </summary>
        [Column("shipment_line_amount")]
        [Range(ShipmentLineValidationRules.AmountMin, ShipmentLineValidationRules.AmountMax)]
        public int Amount { get; set; }

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
        public ShipmentLineEntity() { }

        /// <summary>
        /// 非null許容型の外部キーのエンティティの初期値を設定する
        /// </summary>
        /// <param name="shipment">出荷記録エンティティ</param>
        /// <param name="shipDestination">出荷先名エンティティ</param>
        /// <param name="qualityStandard">品質規格名エンティティ</param>
        /// <param name="unit">単位名エンティティ</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ShipmentLineEntity(ShipmentEntity shipment,
                                  ShipDestinationEntity shipDestination,
                                  QualityStandardEntity qualityStandard,
                                  UnitEntity unit)
        {
            Shipment = shipment ?? throw new ArgumentNullException(nameof(shipment));
            ShipmentId = shipment.Id;
            ShipDestination = shipDestination ?? throw new ArgumentNullException(nameof(shipDestination));
            ShipDestinationId = shipDestination.Id;
            QualityStandard = qualityStandard ?? throw new ArgumentNullException(nameof(qualityStandard));
            QualityStandardId = qualityStandard.Id;
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
            UnitId = unit.Id;
        }
    }
}
