namespace AgriNaviApi.Application.DTOs.Crops
{
    /// <summary>
    /// 作付名削除レスポンス
    /// </summary>
    public class CropDeleteDto
    {
        /// <summary>
        /// 作付名ID(自動インクリメントID)
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
