using AgriNaviApi.Application.Requests.QualityStandards;
using AgriNaviApi.Application.Responses.QualityStandards;
using AgriNaviApi.Application.Responses;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 品質・規格テーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IQualityStandardService
    {
        /// <summary>
        /// 品質・規格テーブルに登録する
        /// </summary>
        /// <param name="request">登録リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<QualityStandardCreateResponse> CreateQualityStandardAsync(
            QualityStandardCreateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 品質・規格テーブル詳細を取得する
        /// </summary>
        /// <param name="id">品質・規格ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<QualityStandardDetailResponse> GetQualityStandardByIdAsync(
            int id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 品質・規格テーブルを更新する
        /// </summary>
        /// <param name="id">品質・規格ID</param>
        /// <param name="request">更新リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<QualityStandardUpdateResponse> UpdateQualityStandardAsync(
            int id,
            QualityStandardUpdateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 品質・規格テーブルから削除する
        /// </summary>
        /// <param name="id">品質・規格ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteQualityStandardAsync(
            int id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 品質・規格テーブルを検索する
        /// </summary>
        /// <param name="request">検索リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<SearchResponse<QualityStandardListItemResponse>> SearchQualityStandardAsync(
            QualityStandardSearchRequest request,
            CancellationToken cancellationToken);
    }
}
