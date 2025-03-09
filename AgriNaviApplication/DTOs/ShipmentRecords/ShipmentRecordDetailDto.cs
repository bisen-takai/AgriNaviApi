using AgriNaviApi.Infrastructure.Persistence.Entities;

namespace AgriNaviApi.Application.DTOs.ShipmentRecords
{
    /// <summary>
    /// 出荷記録詳細レスポンス
    /// </summary>
    public class ShipmentRecordDetailDto
    {
        /// <summary>
        /// 出荷記録ID(自動インクリメントID)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 出荷記録UUID
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// 日付
        /// </summary>
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// 圃場ID
        /// </summary>
        public int FieldId { get; set; }

        /// <summary>
        /// 圃場名
        /// </summary>
        public string? FieldName { get; set; }

        /// <summary>
        /// 作付ID
        /// </summary>
        public int CropId { get; set; }

        /// <summary>
        /// 作付名
        /// </summary>
        public string CropName { get; set; } = string.Empty;

        /// <summary>
        /// 作付年間計画ID
        /// </summary>
        public int SeasonCropScheduleId { get; set; }

        /// <summary>
        /// 作付年間計画名
        /// </summary>
        public string SeasonCropScheduleName { get; set; } = string.Empty;

        /// <summary>
        /// 備考
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最終更新日時
        /// </summary>
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 出荷詳細エンティティのコレクション
        /// </summary>
        public ICollection<ShipmentRecordDetailEntity>? Details { get; set; }
    }
}
