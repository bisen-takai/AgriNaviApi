using AgriNaviApi.Application.Requests.ShipDestinations;
using AgriNaviApi.Application.Responses.ShipDestinations;
using AgriNaviApi.Application.Responses;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 出荷先テーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IShipDestinationService
    {
        /// <summary>
        /// 出荷先テーブルに登録する
        /// </summary>
        /// <param name="request">登録リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<ShipDestinationCreateResponse> CreateShipDestinationAsync(
            ShipDestinationCreateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷先テーブル詳細を取得する
        /// </summary>
        /// <param name="id">出荷先ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<ShipDestinationDetailResponse> GetShipDestinationByIdAsync(
            int id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷先テーブルを更新する
        /// </summary>
        /// <param name="id">出荷先ID</param>
        /// <param name="request">更新リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<ShipDestinationUpdateResponse> UpdateShipDestinationAsync(
            int id,
            ShipDestinationUpdateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷先テーブルから削除する
        /// </summary>
        /// <param name="id">出荷先ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteShipDestinationAsync(
            int id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷先テーブルを検索する
        /// </summary>
        /// <param name="request">検索リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<SearchResponse<ShipDestinationListItemResponse>> SearchShipDestinationAsync(
            ShipDestinationSearchRequest request,
            CancellationToken cancellationToken);
    }
}
