using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Fields
{
    /// <summary>
    /// 圃場削除リクエスト
    /// </summary>
    public class FieldDeleteRequest
    {
        /// <summary>
        /// 圃場ID
        /// </summary>
        [Display(Name = "圃場ID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }
    }
}
