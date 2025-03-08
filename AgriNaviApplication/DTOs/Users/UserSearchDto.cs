namespace AgriNaviApi.Application.DTOs.Users
{
    /// <summary>
    /// ユーザ検索レスポンス
    /// </summary>
    public class UserSearchDto
    {
        /// <summary>
        /// 検索結果のユーザ一覧
        /// </summary>
        public IEnumerable<UserListItemDto> SearchItems { get; set; } = Enumerable.Empty<UserListItemDto>();

        /// <summary>
        /// 検索結果の件数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
