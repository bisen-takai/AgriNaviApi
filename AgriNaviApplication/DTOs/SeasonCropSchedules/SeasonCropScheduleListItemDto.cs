namespace AgriNaviApi.Application.DTOs.SeasonCropSchedules
{
    /// <summary>
    /// 検索結果表示用に必要な情報だけを持つ作付計画情報
    /// </summary>
    public class SeasonCropScheduleListItemDto
    {
        /// <summary>
        /// 作付計画ID(自動インクリメントID)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 作付計画UUID
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// 作付計画名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 作付名ID
        /// </summary>
        public int CropId { get; set; }

        /// <summary>
        /// 作付名
        /// </summary>
        public string CropName { get; set; } = string.Empty;

        /// <summary>
        /// 計画開始年月
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 計画終了年月
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        public string? Remark { get; set; }
    }
}
