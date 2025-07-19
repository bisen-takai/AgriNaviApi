using AgriNaviApi.Application.Responses.ShipmentLines;

namespace AgriNaviApi.Application.Responses.ShipmentWithLines
{
    /// <summary>
    /// 出荷記録レスポンスの基底クラス
    /// </summary>
    public abstract record ShipmentWithLineBaseResponse
    {
        /// <summary>
        /// 出荷記録UUID
        /// </summary>
        public Guid Uuid { get; init; }

        /// <summary>
        /// 出荷日付
        /// </summary>
        public DateOnly ShipmentDate { get; init; }

        /// <summary>
        /// 圃場ID
        /// </summary>
        public int FieldId { get; init; }

        /// <summary>
        /// 圃場名
        /// </summary>
        public string? FieldName { get; init; }

        /// <summary>
        /// 作付年間計画ID
        /// </summary>
        public int SeasonScheduleId { get; init; }

        /// <summary>
        /// 作付年間計画名
        /// </summary>
        public string SeasonScheduleName { get; init; } = string.Empty;

        /// <summary>
        /// 作付名
        /// </summary>
        public string CropName { get; init; } = string.Empty;

        /// <summary>
        /// 備考
        /// </summary>
        public string? Remark { get; init; }

        /// <summary>
        /// 出荷詳細エンティティのコレクション
        /// </summary>
        public ICollection<ShipmentLineBaseResponse>? Details { get; set; } = new List<ShipmentLineBaseResponse>();
    }
}
