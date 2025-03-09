namespace AgriNaviApi.Application.DTOs.ShipmentRecordDetails
{
    /// <summary>
    /// 検索結果表示用に必要な情報だけを持つ出荷記録詳細情報
    /// </summary>
    public class ShipmentRecordDetailListItemDto
    {
        /// <summary>
        /// 出荷記録詳細ID(自動インクリメントID)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 出荷記録詳細UUID
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// 出荷記録ID
        /// </summary>
        public int ShipmentRecordId { get; set; }

        /// <summary>
        /// 出荷先ID
        /// </summary>
        public int ShippingDestinationId { get; set; }

        /// <summary>
        /// 出荷先名
        /// </summary>
        public string ShippingDestinationName { get; set; } = string.Empty;

        /// <summary>
        /// 品質規格ID
        /// </summary>
        public int QualityStandardId { get; set; }

        /// <summary>
        /// 品質規格名
        /// </summary>
        public string QualityStandardName { get; set; } = string.Empty;

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 単位ID
        /// </summary>
        public int UnitId { get; set; }

        /// <summary>
        /// 単位名
        /// </summary>
        public string UnitName { get; set; } = string.Empty;

        /// <summary>
        /// 金額
        /// </summary>
        public int Amount { get; set; }
    }
}
