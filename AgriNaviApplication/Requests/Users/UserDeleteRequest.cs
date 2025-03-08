using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Users
{
    /// <summary>
    /// ユーザ削除リクエスト
    /// </summary>
    public class UserDeleteRequest
    {
        /// <summary>
        /// ユーザID
        /// </summary>
        [Display(Name = "ユーザID")]
        [Required(ErrorMessageResourceName = nameof(CommonValidationMessages.RequiredMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public int Id { get; set; }
    }
}
