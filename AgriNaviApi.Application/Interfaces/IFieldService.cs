using AgriNaviApi.Application.Requests.Fields;
using AgriNaviApi.Application.Responses.Fields;
using AgriNaviApi.Application.Responses;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 圃場テーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IFieldService
    {
        /// <summary>
        /// 圃場テーブルに登録する
        /// </summary>
        /// <param name="request">登録リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<FieldCreateResponse> CreateFieldAsync(
            FieldCreateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 圃場テーブル詳細を取得する
        /// </summary>
        /// <param name="id">圃場ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<FieldDetailResponse> GetFieldByIdAsync(
            int id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 圃場テーブルを更新する
        /// </summary>
        /// <param name="id">圃場ID</param>
        /// <param name="request">更新リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<FieldUpdateResponse> UpdateFieldAsync(
            int id,
            FieldUpdateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 圃場テーブルから削除する
        /// </summary>
        /// <param name="id">圃場ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteFieldAsync(
            int id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 圃場テーブルを検索する
        /// </summary>
        /// <param name="request">検索リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<SearchResponse<FieldListItemResponse>> SearchFieldAsync(
            FieldSearchRequest request,
            CancellationToken cancellationToken);
    }
}
