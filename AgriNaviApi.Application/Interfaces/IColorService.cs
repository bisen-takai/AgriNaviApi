using AgriNaviApi.Application.Requests.Colors;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.Colors;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// カラーテーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IColorService
    {
        /// <summary>
        /// カラーテーブルに登録する
        /// </summary>
        /// <param name="request">登録リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<ColorCreateResponse> CreateColorAsync(
            ColorCreateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// カラーテーブル詳細を取得する
        /// </summary>
        /// <param name="id">カラーID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<ColorDetailResponse> GetColorByIdAsync(
            int id,
            CancellationToken cancellationToken);

        /// <summary>
        /// カラーテーブルを更新する
        /// </summary>
        /// <param name="id">カラーID</param>
        /// <param name="request">更新リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<ColorUpdateResponse> UpdateColorAsync(
            int id,
            ColorUpdateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// カラーテーブルから削除する
        /// </summary>
        /// <param name="id">カラーID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteColorAsync(
            int id,
            CancellationToken cancellationToken);

        /// <summary>
        /// カラーテーブルを検索する
        /// </summary>
        /// <param name="request">検索リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<SearchResponse<ColorListItemResponse>> SearchColorAsync(
            ColorSearchRequest request,
            CancellationToken cancellationToken);
    }
}
