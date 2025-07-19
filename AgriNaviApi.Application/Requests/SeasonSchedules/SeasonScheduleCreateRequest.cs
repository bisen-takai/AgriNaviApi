using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.SeasonSchedules
{
    /// <summary>
    /// 作付計画登録時のリクエスト
    /// </summary>
    [DateRangeValidation("StartDate", "EndDate")]
    public class SeasonScheduleCreateRequest
    {
        /// <summary>
        /// 作付計画名
        /// </summary>
        [Display(Name = "作付計画名")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(SeasonScheduleValidationRules.NameMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [TrimmedNotEmpty(ErrorMessageResourceName = nameof(CommonValidationMessages.InvalidFormatMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 作付ID
        /// </summary>
        [Display(Name = "作付ID")]
        [Range(1, int.MaxValue, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int CropId { get; set; }

        /// <summary>
        /// 計画開始年月
        /// </summary>
        [Display(Name = "計画開始年月日")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// 計画終了年月
        /// </summary>
        [Display(Name = "計画終了年月日")]
        [DataType(DataType.Date)]
        public DateOnly? EndDate { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Display(Name = "備考")]
        [StringLength(CommonValidationRules.RemarkMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? Remark { get; set; }
    }
}
