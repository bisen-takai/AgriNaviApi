namespace AgriNaviApi.Application.DTOs.SeasonCropSchedules
{
    /// <summary>
    /// 作付計画登録レスポンス
    /// </summary>
    public class SeasonCropScheduleCreateDto
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

        /// <summary>
        /// 登録日時
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最終更新日時
        /// </summary>
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
