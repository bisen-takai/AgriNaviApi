using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Groups
{
    /// <summary>
    /// グループ削除リクエスト
    /// </summary>
    public class GroupDeleteRequest
    {
        /// <summary>
        /// グループID
        /// </summary>
        [Display(Name = "グループID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }
    }
}
