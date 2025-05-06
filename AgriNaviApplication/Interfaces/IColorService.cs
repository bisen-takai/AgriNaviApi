using AgriNaviApi.Application.DTOs.Colors;
using AgriNaviApi.Application.Requests.Colors;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// カラーテーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IColorService
    {
        /// <summary>
        /// カラーテーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ColorCreateDto> CreateColorAsync(ColorCreateRequest request);

        /// <summary>
        /// カラーテーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<ColorDetailDto> GetColorByIdAsync(int id);

        /// <summary>
        /// カラーテーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ColorUpdateDto> UpdateColorAsync(ColorUpdateRequest request);

        /// <summary>
        /// カラーテーブルから削除する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ColorDeleteDto> DeleteColorAsync(int id);

        /// <summary>
        /// カラーテーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ColorSearchDto> SearchColorAsync(ColorSearchRequest request); 
    }
}
