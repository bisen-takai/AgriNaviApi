using AgriNaviApi.Application.DTOs.Crops;
using AgriNaviApi.Application.Requests.Crops;

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
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<CropCreateDto> CreateCropAsync(CropCreateRequest request);

        /// <summary>
        /// 作付テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<CropDetailDto> GetCropByIdAsync(int id);

        /// <summary>
        /// 作付テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<CropUpdateDto> UpdateCropAsync(CropUpdateRequest request);

        /// <summary>
        /// 作付テーブルから削除する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<CropDeleteDto> DeleteCropAsync(int id);

        /// <summary>
        /// 作付テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<CropSearchDto> SearchCropAsync(CropSearchRequest request);
    }
}
