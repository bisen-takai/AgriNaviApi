using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Application.Requests.Users
{
    /// <summary>
    /// ユーザ検索リクエスト
    /// </summary>
    public class UserSearchRequest
    {
        /// <summary>
        /// ユーザ名
        /// </summary>
        [Display(Name = "ユーザ名")]
        public string? SearchUserName { get; set; }

        /// <summary>
        /// 検索一致タイプ
        /// </summary>
        [Display(Name = "検索一致タイプ")]
        [Range((int)SearchMatchType.None, (int)SearchMatchType.PARTIAL, ErrorMessageResourceName = nameof(CommonValidationMessages.SearchMatchTypeMessage), ErrorMessageResourceType = typeof(CommonValidationMessages))]
        public SearchMatchType SearchMatchType { get; set; } = SearchMatchType.None;
    }
}
