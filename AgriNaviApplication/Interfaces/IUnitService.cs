using AgriNaviApi.Application.DTOs.Units;
using AgriNaviApi.Application.Requests.Units;

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
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<UnitCreateDto> CreateUnitAsync(UnitCreateRequest request);

        /// <summary>
        /// 単位テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<UnitDetailDto> GetUnitByIdAsync(int id);

        /// <summary>
        /// 単位テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<UnitUpdateDto> UpdateUnitAsync(UnitUpdateRequest request);

        /// <summary>
        /// 単位テーブルから削除する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<UnitDeleteDto> DeleteUnitAsync(UnitDeleteRequest request);

        /// <summary>
        /// 単位テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<UnitSearchDto> SearchUnitAsync(UnitSearchRequest request);
    }
}
