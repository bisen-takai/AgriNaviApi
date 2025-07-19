using AgriNaviApi.Application.Requests.Units;
using AgriNaviApi.Application.Responses.Units;
using AgriNaviApi.Application.Responses;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 単位テーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IUnitService
    {
        /// <summary>
        /// 単位テーブルに登録する
        /// </summary>
        /// <param name="request">登録リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<UnitCreateResponse> CreateUnitAsync(
            UnitCreateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 単位テーブル詳細を取得する
        /// </summary>
        /// <param name="id">単位ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<UnitDetailResponse> GetUnitByIdAsync(
            int id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 単位テーブルを更新する
        /// </summary>
        /// <param name="id">単位ID</param>
        /// <param name="request">更新リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<UnitUpdateResponse> UpdateUnitAsync(
            int id,
            UnitUpdateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 単位テーブルから削除する
        /// </summary>
        /// <param name="id">単位ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteUnitAsync(
            int id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 単位テーブルを検索する
        /// </summary>
        /// <param name="request">検索リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<SearchResponse<UnitListItemResponse>> SearchUnitAsync(
            UnitSearchRequest request,
            CancellationToken cancellationToken);
    }
}
