using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Crops
{
    /// <summary>
    /// 作付名更新リクエスト
    /// </summary>
    public class CropUpdateRequest
    {
        /// <summary>
        /// 作付ID
        /// </summary>
        [Display(Name = "作付ID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }

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
        public string? ShortenName { get; set; }

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
        [StringLength(200, ErrorMessageResourceName = nameof(CommonValidationMessages.MaxLengthMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public string? Remark { get; set; }
    }
}
