using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Crops
{
    /// <summary>
    /// 作付追加リクエスト
    /// </summary>
    public class CropCreateRequest
    {
        /// <summary>
        /// 作付名
        /// </summary>
        [Display(Name = "作付名")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(20, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 作付名省略名
        /// </summary>
        [Display(Name = "作付名省略名")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        [StringLength(4, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? ShortName { get; set; }

        /// <summary>
        /// グループID
        /// </summary>
        [Display(Name = "グループID")]
        public int GroupId { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        [Display(Name = "カラーID")]
        public int ColorId { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        [Display(Name = "備考")]
        [StringLength(200, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? Remark { get; set; }
    }
}
