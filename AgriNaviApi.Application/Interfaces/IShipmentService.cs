using AgriNaviApi.Application.Requests.Shipments;
using AgriNaviApi.Application.Responses.Shipments;
using AgriNaviApi.Application.Responses;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 出荷記録テーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IShipmentService
    {
        /// <summary>
        /// 出荷記録テーブルに登録する
        /// </summary>
        /// <param name="request">登録リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<ShipmentCreateResponse> CreateShipmentAsync(
            ShipmentCreateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷記録テーブル詳細を取得する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<ShipmentDetailResponse> GetShipmentByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷記録テーブルを更新する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <param name="request">更新リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<ShipmentUpdateResponse> UpdateShipmentAsync(
            Guid id,
            ShipmentUpdateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷記録テーブルから削除する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<DeleteWithUuidResponse> DeleteShipmentAsync(
            Guid id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷記録テーブルを検索する
        /// </summary>
        /// <param name="request">検索リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<SearchResponse<ShipmentListItemResponse>> SearchShipmentAsync(
            ShipmentSearchRequest request,
            CancellationToken cancellationToken);
    }
}
