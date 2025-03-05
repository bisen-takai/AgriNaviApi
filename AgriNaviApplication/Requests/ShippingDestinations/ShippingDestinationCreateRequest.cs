using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.ShippingDestinations
{
    /// <summary>
    /// 出荷先名追加リクエスト
    /// </summary>
    public class ShippingDestinationCreateRequest
    {
        /// <summary>
        /// 出荷先名
        /// </summary>
        [Display(Name = "出荷先名")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(20, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string Name { get; set; } = string.Empty;
    }
}
