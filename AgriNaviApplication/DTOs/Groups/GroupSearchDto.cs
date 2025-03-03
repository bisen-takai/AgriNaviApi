namespace AgriNaviApi.Application.DTOs.Groups
{
    /// <summary>
    /// グループ検索レスポンス
    /// </summary>
    public class GroupSearchDto
    {
        /// <summary>
        /// 検索結果のグループ一覧
        /// </summary>
        public IEnumerable<GroupListItemDto> SearchItems { get; set; } = Enumerable.Empty<GroupListItemDto>();

        /// <summary>
        /// 検索結果の件数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
