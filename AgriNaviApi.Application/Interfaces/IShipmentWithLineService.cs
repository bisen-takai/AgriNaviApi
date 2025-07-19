using AgriNaviApi.Application.Requests.ShipmentWithLines;
using AgriNaviApi.Application.Responses.ShipmentWithLines;
using AgriNaviApi.Application.Responses;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 出荷記録テーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IShipmentWithLineService
    {
        /// <summary>
        /// 出荷記録テーブルに登録する
        /// </summary>
        /// <param name="request">登録リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<ShipmentWithLineCreateResponse> CreateShipmentWithLineAsync(
            ShipmentWithLineCreateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷記録テーブル詳細を取得する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<ShipmentWithLineDetailResponse> GetShipmentWithLineByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷記録テーブルを更新する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <param name="request">更新リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<ShipmentWithLineUpdateResponse> UpdateShipmentWithLineAsync(
            Guid id,
            ShipmentWithLineUpdateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷記録テーブルから削除する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<DeleteWithUuidResponse> DeleteShipmentWithLineAsync(
            Guid id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷記録テーブルを検索する
        /// </summary>
        /// <param name="request">検索リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<SearchResponse<ShipmentWithLineListItemResponse>> SearchShipmentWithLineAsync(
            ShipmentWithLineSearchRequest request,
            CancellationToken cancellationToken);
    }
}
