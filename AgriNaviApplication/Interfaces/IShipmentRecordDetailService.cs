using AgriNaviApi.Application.DTOs.ShipmentRecordDetails;
using AgriNaviApi.Application.Requests.ShipmentRecordDetails;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 出荷記録詳細テーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IShipmentRecordDetailService
    {
        /// <summary>
        /// 出荷記録詳細テーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DuplicateEntityException"></exception>
        public Task<ShipmentRecordDetailCreateDto> CreateShipmentRecordDetailAsync(ShipmentRecordDetailCreateRequest request);

        /// <summary>
        /// 出荷記録詳細テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public Task<ShipmentRecordDetailDetailDto> GetShipmentRecordDetailByIdAsync(int id);

        /// <summary>
        /// 出荷記録詳細テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public Task<ShipmentRecordDetailUpdateDto> UpdateShipmentRecordDetailAsync(ShipmentRecordDetailUpdateRequest request);

        /// <summary>
        /// 出荷記録詳細テーブルから削除する※削除フラグをtrueにするのみで、実際は削除しない
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Task<ShipmentRecordDetailDeleteDto> DeleteShipmentRecordDetailAsync(ShipmentRecordDetailDeleteRequest request);

        /// <summary>
        /// 出荷記録詳細テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ShipmentRecordDetailSearchDto> SearchShipmentRecordDetailAsync(ShipmentRecordDetailSearchRequest request);
    }
}
