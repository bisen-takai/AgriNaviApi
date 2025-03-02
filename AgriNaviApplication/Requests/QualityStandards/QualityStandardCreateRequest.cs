using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.QualityStandards
{
    /// <summary>
    /// 品質・規格追加リクエスト
    /// </summary>
    public class QualityStandardCreateRequest
    {
        /// <summary>
        /// 品質・規格名
        /// </summary>
        [Display(Name = "品質・規格名")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(20, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string Name { get; set; } = string.Empty;
    }
}
