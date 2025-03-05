namespace AgriNaviApi.Application.DTOs.ShippingDestinations
{
    /// <summary>
    /// 出荷先名削除レスポンス
    /// </summary>
    public class ShippingDestinationDeleteDto
    {
        /// <summary>
        /// 出荷先名ID(自動インクリメントID)
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
