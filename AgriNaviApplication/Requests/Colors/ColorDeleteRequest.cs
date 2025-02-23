using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Colors
{
    /// <summary>
    /// カラー削除リクエスト
    /// </summary>
    public class ColorDeleteRequest
    {
        /// <summary>
        /// カラーID
        /// </summary>
        [Display(Name = "カラーID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }
    }
}
