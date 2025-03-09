using AgriNaviApi.Application.DTOs.ShipmentRecords;
using AgriNaviApi.Application.Requests.ShipmentRecords;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 出荷記録テーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IShipmentRecordService
    {
        /// <summary>
        /// 出荷記録テーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DuplicateEntityException"></exception>
        public Task<ShipmentRecordCreateDto> CreateShipmentRecordAsync(ShipmentRecordCreateRequest request);

        /// <summary>
        /// 出荷記録テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public Task<ShipmentRecordDetailDto> GetShipmentRecordByIdAsync(int id);

        /// <summary>
        /// 出荷記録テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public Task<ShipmentRecordUpdateDto> UpdateShipmentRecordAsync(ShipmentRecordUpdateRequest request);

        /// <summary>
        /// 出荷記録テーブルから削除する※削除フラグをtrueにするのみで、実際は削除しない
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Task<ShipmentRecordDeleteDto> DeleteShipmentRecordAsync(ShipmentRecordDeleteRequest request);

        /// <summary>
        /// 出荷記録テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ShipmentRecordSearchDto> SearchShipmentRecordAsync(ShipmentRecordSearchRequest request);
    }
}
