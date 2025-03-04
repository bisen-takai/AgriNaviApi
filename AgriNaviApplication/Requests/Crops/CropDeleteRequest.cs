using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Crops
{
    /// <summary>
    /// 作付削除リクエスト
    /// </summary>
    public class CropDeleteRequest
    {
        /// <summary>
        /// 作付ID
        /// </summary>
        [Display(Name = "作付ID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }
    }
}
