using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Colors
{
    /// <summary>
    /// カラー更新時のリクエスト
    /// </summary>
    public class ColorUpdateRequest
    {
        /// <summary>
        /// カラー名
        /// </summary>
        [Display(Name = "カラー名")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(ColorValidationRules.NameMax, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [TrimmedNotEmpty(ErrorMessageResourceName = nameof(CommonValidationMessages.InvalidFormatMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Red値
        /// </summary>
        [Display(Name = "Red値")]
        [Range(ColorValidationRules.ColorValueMin, ColorValidationRules.ColorValueMax, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int RedValue { get; set; }

        /// <summary>
        /// Green値
        /// </summary>
        [Display(Name = "Green値")]
        [Range(ColorValidationRules.ColorValueMin, ColorValidationRules.ColorValueMax, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int GreenValue { get; set; }

        /// <summary>
        /// Blue値
        /// </summary>
        [Display(Name = "Blue値")]
        [Range(ColorValidationRules.ColorValueMin, ColorValidationRules.ColorValueMax, ErrorMessageResourceName = nameof(CommonValidationMessages.RangeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int BlueValue { get; set; }
    }
}
