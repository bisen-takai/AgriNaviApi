namespace AgriNaviApi.Application.DTOs.ShipmentRecords
{
    /// <summary>
    /// 出荷記録削除レスポンス
    /// </summary>
    public class ShipmentRecordWithDetailDeleteDto
    {
        /// <summary>
        /// 出荷記録ID(自動インクリメントID)
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
