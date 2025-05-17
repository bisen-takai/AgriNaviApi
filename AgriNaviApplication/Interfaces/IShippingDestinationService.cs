using AgriNaviApi.Application.DTOs.ShippingDestinations;
using AgriNaviApi.Application.Requests.ShippingDestinations;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 出荷先名テーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IShippingDestinationService
    {
        /// <summary>
        /// 出荷先名テーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ShippingDestinationCreateDto> CreateShippingDestinationAsync(ShippingDestinationCreateRequest request);

        /// <summary>
        /// 出荷先名テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ShippingDestinationDetailDto> GetShippingDestinationByIdAsync(int id);

        /// <summary>
        /// 出荷先名テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ShippingDestinationUpdateDto> UpdateShippingDestinationAsync(ShippingDestinationUpdateRequest request);

        /// <summary>
        /// 出荷先名テーブルから削除する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ShippingDestinationDeleteDto> DeleteShippingDestinationAsync(int id);

        /// <summary>
        /// 出荷先名テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ShippingDestinationSearchDto> SearchShippingDestinationAsync(ShippingDestinationSearchRequest request);
    }
}
