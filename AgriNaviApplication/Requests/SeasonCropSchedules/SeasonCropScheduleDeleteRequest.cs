using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.SeasonCropSchedules
{
    /// <summary>
    /// 作付計画削除リクエスト
    /// </summary>
    public class SeasonCropScheduleDeleteRequest
    {
        /// <summary>
        /// 作付計画ID
        /// </summary>
        [Display(Name = "作付計画ID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }
    }
}
