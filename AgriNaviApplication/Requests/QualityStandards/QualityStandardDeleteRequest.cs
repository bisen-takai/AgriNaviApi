using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.QualityStandards
{
    /// <summary>
    /// 品質・規格削除リクエスト
    /// </summary>
    public class QualityStandardDeleteRequest
    {
        /// <summary>
        /// 品質・規格ID
        /// </summary>
        [Display(Name = "品質・規格ID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }
    }
}
