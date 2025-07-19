namespace AgriNaviApi.Application.Responses
{
    /// <summary>
    /// 検索時のレスポンス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SearchResponse<T>
    {
        /// <summary>
        /// 検索結果の一覧
        /// </summary>
        public IEnumerable<T> SearchItems { get; set; } = Enumerable.Empty<T>();

        /// <summary>
        /// 検索結果の件数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 現在のページ番号(1から開始)
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 1ページあたりの表示件数
        /// </summary>
        public int PageSize { get; set; }
    }
}
