using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Units
{
    /// <summary>
    /// 単位削除リクエスト
    /// </summary>
    public class UnitDeleteRequest
    {
        /// <summary>
        /// 単位ID
        /// </summary>
        [Display(Name = "単位ID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }
    }
}
