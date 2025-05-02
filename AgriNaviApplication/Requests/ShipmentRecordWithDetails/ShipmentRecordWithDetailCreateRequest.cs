using AgriNaviApi.Application.Requests.ShipmentRecordDetails;
using AgriNaviApi.Common.Resources;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.ShipmentRecords
{
    /// <summary>
    /// 出荷記録追加リクエスト
    /// </summary>
    public class ShipmentRecordWithDetailCreateRequest
    {
        /// <summary>
        /// 日付
        /// </summary>
        [Display(Name = "日付")]
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// 圃場ID
        /// </summary>
        [Display(Name = "圃場ID")]
        public int FieldId { get; set; }

        /// <summary>
        /// 作付ID
        /// </summary>
        [Display(Name = "作付ID")]
        public int CropId { get; set; }

        /// <summary>
        /// 作付年間計画ID
        /// </summary>
        [Display(Name = "作付年間計画ID")]
        public int SeasonCropScheduleId { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Display(Name = "備考")]
        [StringLength(200, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? Remark { get; set; }

        /// <summary>
        /// 出荷詳細エンティティのコレクション
        /// </summary>
        [Display(Name = "出荷詳細")]
        public ICollection<ShipmentRecordDetailCreateRequest> Details { get; set; } = new List<ShipmentRecordDetailCreateRequest>();
    }
}
