using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// 出荷記録詳細テーブル
    /// </summary>
    [Table("shipment_record_details")]
    public class ShipmentRecordDetailEntity
    {
        /// <summary>
        /// 出荷記録詳細ID(自動インクリメントID)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("shipment_record_detail_id")]
        public int Id { get; set; }

        /// <summary>
        /// 出荷記録詳細UUID
        /// </summary>
        [Column("shipment_record_detail_uuid")]
        public Guid Uuid { get; set; }

        /// <summary>
        /// 出荷記録ID
        /// </summary>
        [Column("shipment_record_id")]
        public int ShipmentRecordId { get; set; }

        /// <summary>
        /// 出荷記録エンティティ
        /// </summary>
        [ForeignKey(nameof(ShipmentRecordId))]
        public ShipmentRecordEntity ShipmentRecord { get; set; }

        /// <summary>
        /// 出荷先ID
        /// </summary>
        [Column("shipping_destination_id")]
        public int ShippingDestinationId { get; set; }

        /// <summary>
        /// 出荷先エンティティ
        /// </summary>
        [ForeignKey(nameof(ShippingDestinationId))]
        public ShippingDestinationEntity ShippingDestination { get; set; }

        /// <summary>
        /// 品質規格ID
        /// </summary>
        [Column("quality_standard_id")]
        public int QualityStandardId { get; set; }

        /// <summary>
        /// 品質規格エンティティ
        /// </summary>
        [ForeignKey(nameof(QualityStandardId))]
        public QualityStandardEntity QualityStandard { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Column("shipment_record_detail_quantity")]
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
        public UnitEntity Unit { get; set; }

        /// <summary>
        /// 金額
        /// </summary>
        [Column("shipment_record_detail_amount")]
        public int Amount { get; set; }

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
        /// 非null許容型の外部キーのエンティティの初期値がない場合はnullを設定する
        /// </summary>
        public ShipmentRecordDetailEntity()
        {
            ShipmentRecord = null!;
            ShippingDestination = null!;
            QualityStandard = null!;
            Unit = null!;
        }

        /// <summary>
        /// 非null許容型の外部キーのエンティティの初期値を設定する
        /// </summary>
        /// <param name="shipmentRecord">出荷記録エンティティ</param>
        /// <param name="shippingDestination">出荷先名エンティティ</param>
        /// <param name="qualityStandard">品質規格名エンティティ</param>
        /// <param name="unit">単位名エンティティ</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ShipmentRecordDetailEntity(ShipmentRecordEntity shipmentRecord,
                                                     ShippingDestinationEntity shippingDestination,
                                                     QualityStandardEntity qualityStandard,
                                                     UnitEntity unit)
        {
            ShipmentRecord = shipmentRecord ?? throw new ArgumentNullException(nameof(shipmentRecord));
            ShippingDestination = shippingDestination ?? throw new ArgumentNullException(nameof(shippingDestination));
            QualityStandard = qualityStandard ?? throw new ArgumentNullException(nameof(qualityStandard));
            Unit = unit ?? throw new ArgumentNullException(nameof(unit));
        }
    }
}
