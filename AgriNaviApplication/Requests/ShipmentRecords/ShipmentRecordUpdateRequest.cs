using AgriNaviApi.Common.Resources;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.ShipmentRecords
{
    /// <summary>
    /// 出荷記録更新リクエスト
    /// </summary>
    public class ShipmentRecordUpdateRequest
    {
        /// <summary>
        /// 出荷記録ID
        /// </summary>
        [Display(Name = "出荷記録ID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }

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
    }
}
