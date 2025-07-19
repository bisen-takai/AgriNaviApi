using AgriNaviApi.Application.Requests.ShipmentLines;
using AgriNaviApi.Application.Responses.ShipmentLines;
using AgriNaviApi.Application.Responses;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 出荷記録詳細テーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IShipmentLineService
    {
        /// <summary>
        /// 出荷記録詳細テーブルに登録する
        /// </summary>
        /// <param name="request">登録リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<ShipmentLineCreateResponse> CreateShipmentLineAsync(
            ShipmentLineCreateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷記録詳細テーブル詳細を取得する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<ShipmentLineDetailResponse> GetShipmentLineByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷記録詳細テーブルを更新する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <param name="request">更新リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<ShipmentLineUpdateResponse> UpdateShipmentLineAsync(
            Guid id,
            ShipmentLineUpdateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷記録詳細テーブルから削除する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<DeleteWithUuidResponse> DeleteShipmentLineAsync(
            Guid id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 出荷記録詳細テーブルを検索する
        /// </summary>
        /// <param name="request">検索リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<SearchResponse<ShipmentLineListItemResponse>> SearchShipmentLineAsync(
            ShipmentLineSearchRequest request,
            CancellationToken cancellationToken);
    }
}
