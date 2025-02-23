using AgriNaviApi.Application.DTOs.Colors;
using AgriNaviApi.Application.Requests.Colors;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// カラーテーブルに関するサービス処理
    /// </summary>
    public interface IColorService
    {
        /// <summary>
        /// カラーテーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ColorCreateDto> CreateColorAsync(ColorCreateRequest request);

        /// <summary>
        /// カラーテーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ColorUpdateDto> UpdateColorAsync(ColorUpdateRequest request);

        /// <summary>
        /// カラーテーブルから削除する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ColorDeleteDto> DeleteColorAsync(ColorDeleteRequest request);

        /// <summary>
        /// カラーテーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ColorSearchDto> SearchColorAsync(ColorSearchRequest request); 
    }
}
