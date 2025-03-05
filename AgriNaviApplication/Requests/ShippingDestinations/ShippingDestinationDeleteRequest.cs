using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.ShippingDestinations
{
    /// <summary>
    /// 出荷先名削除リクエスト
    /// </summary>
    public class ShippingDestinationDeleteRequest
    {
        /// <summary>
        /// 出荷先名ID
        /// </summary>
        [Display(Name = "出荷先名ID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }
    }
}
