﻿using AgriNaviApi.Common.Resources;
using AgriNaviApi.Common.Validations;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.SeasonCropSchedules
{
    /// <summary>
    /// 作付計画追加リクエスト
    /// </summary>
    [DateRangeValidation("StartDate", "EndDate")]
    public class SeasonCropScheduleCreateRequest
    {
        /// <summary>
        /// 作付計画名
        /// </summary>
        [Display(Name = "作付計画名")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(30, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 作付名ID
        /// </summary>
        [Display(Name = "作付名ID")]
        public int CropId { get; set; }

        /// <summary>
        /// 計画開始年月
        /// </summary>
        [Display(Name = "計画開始年月")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 計画終了年月
        /// </summary>
        [Display(Name = "計画終了年月")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Display(Name = "備考")]
        [StringLength(200, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? Remark { get; set; }
    }
}
