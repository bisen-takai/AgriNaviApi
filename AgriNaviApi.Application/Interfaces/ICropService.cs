using AgriNaviApi.Application.Requests.Crops;
using AgriNaviApi.Application.Responses.Crops;
using AgriNaviApi.Application.Responses;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 作付テーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface ICropService
    {
        /// <summary>
        /// 作付テーブルに登録する
        /// </summary>
        /// <param name="request">登録リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<CropCreateResponse> CreateCropAsync(
            CropCreateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 作付テーブル詳細を取得する
        /// </summary>
        /// <param name="id">作付ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<CropDetailResponse> GetCropByIdAsync(
            int id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 作付テーブルを更新する
        /// </summary>
        /// <param name="id">作付ID</param>
        /// <param name="request">更新リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<CropUpdateResponse> UpdateCropAsync(
            int id,
            CropUpdateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 作付テーブルから削除する
        /// </summary>
        /// <param name="id">作付ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteCropAsync(
            int id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 作付テーブルを検索する
        /// </summary>
        /// <param name="request">検索リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<SearchResponse<CropListItemResponse>> SearchCropAsync(
            CropSearchRequest request,
            CancellationToken cancellationToken);
    }
}
