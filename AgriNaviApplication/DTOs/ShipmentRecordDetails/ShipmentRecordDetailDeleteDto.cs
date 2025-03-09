namespace AgriNaviApi.Application.DTOs.ShipmentRecordDetails
{
    /// <summary>
    /// 出荷記録詳細削除レスポンス
    /// </summary>
    public class ShipmentRecordDetailDeleteDto
    {
        /// <summary>
        /// 出荷記録詳細ID(自動インクリメントID)
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
