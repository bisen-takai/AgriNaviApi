using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.ShipmentRecordDetails
{
    /// <summary>
    /// 出荷記録詳細検索リクエスト
    /// </summary>
    public class ShipmentRecordDetailSearchRequest
    {

        /// <summary>
        /// 出荷記録ID
        /// </summary>
        [Display(Name = "出荷記録ID")]
        public int ShipmentRecordId { get; set; }
    }
}
