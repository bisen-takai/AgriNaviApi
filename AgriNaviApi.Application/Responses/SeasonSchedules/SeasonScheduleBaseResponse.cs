using AgriNaviApi.Infrastructure.Persistence.Entities;

namespace AgriNaviApi.Application.Responses.SeasonSchedules
{
    /// <summary>
    /// 作付計画レスポンスの基底クラス
    /// </summary>
    public abstract record SeasonScheduleBaseResponse
    {
        /// <summary>
        /// 作付計画ID(自動インクリメントID)
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// 作付計画UUID（アプリ側から直接設定不可。SaveChanges内で自動設定）
        /// </summary>
        public Guid Uuid { get; private set; }

        /// <summary>
        /// 作付計画名
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 作付ID
        /// </summary>
        public int CropId { get; set; }

        /// <summary>
        /// 作付名
        /// </summary>
        public CropEntity Crop { get; set; } = null!;

        /// <summary>
        /// 計画開始年月日
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// 計画終了年月日
        /// </summary>
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        public string? Remark { get; set; }
    }
}
