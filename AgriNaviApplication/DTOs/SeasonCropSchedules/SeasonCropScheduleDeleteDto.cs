namespace AgriNaviApi.Application.DTOs.SeasonCropSchedules
{
    /// <summary>
    /// 作付計画削除レスポンス
    /// </summary>
    public class SeasonCropScheduleDeleteDto
    {
        /// <summary>
        /// 作付計画ID(自動インクリメントID)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 削除成功か
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 削除結果に関するメッセージ
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
