using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Colors
{
    /// <summary>
    /// カラー更新リクエスト
    /// </summary>
    public class ColorUpdateRequest
    {
        /// <summary>
        /// カラーID
        /// </summary>
        [Display(Name = "カラーID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        [Display(Name = "カラー名")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(20, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Red値
        /// </summary>
        [Display(Name = "Red値")]
        [Range(0, 255, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int RedValue { get; set; }

        /// <summary>
        /// Green値
        /// </summary>
        [Display(Name = "Green値")]
        [Range(0, 255, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int GreenValue { get; set; }

        /// <summary>
        /// Blue値
        /// </summary>
        [Display(Name = "Blue値")]
        [Range(0, 255, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int BlueValue { get; set; }
    }
}
