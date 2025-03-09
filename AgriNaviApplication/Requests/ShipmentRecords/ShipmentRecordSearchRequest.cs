using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.ShipmentRecords
{
    /// <summary>
    /// 出荷記録検索リクエスト
    /// </summary>
    public class ShipmentRecordSearchRequest
    {
        /// <summary>
        /// 出荷記録開始日
        /// </summary>
        [Display(Name = "出荷記録開始日")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 出荷記録終了日
        /// </summary>
        [Display(Name = "出荷記録終了日")]
        public DateTime? EndDate { get; set; }
    }
}
