using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Units
{
    /// <summary>
    /// 単位更新時のリクエスト
    /// </summary>
    public class UnitUpdateRequest
    {
        /// <summary>
        /// 単位名
        /// </summary>
        [Display(Name = "単位名")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(UnitValidationRules.NameMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [TrimmedNotEmpty(ErrorMessageResourceName = nameof(CommonValidationMessages.InvalidFormatMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string Name { get; set; } = string.Empty;
    }
}
