using AgriNaviApi.Application.DTOs.QualityStandards;
using AgriNaviApi.Application.Requests.QualityStandards;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 品質・規格名テーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IQualityStandardService
    {
        /// <summary>
        /// 品質・規格名テーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<QualityStandardCreateDto> CreateQualityStandardAsync(QualityStandardCreateRequest request);

        /// <summary>
        /// 品質・規格名テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<QualityStandardDetailDto> GetQualityStandardByIdAsync(int id);

        /// <summary>
        /// 品質・規格名テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<QualityStandardUpdateDto> UpdateQualityStandardAsync(QualityStandardUpdateRequest request);

        /// <summary>
        /// 品質・規格名テーブルから削除する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<QualityStandardDeleteDto> DeleteQualityStandardAsync(QualityStandardDeleteRequest request);

        /// <summary>
        /// 品質・規格名テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<QualityStandardSearchDto> SearchQualityStandardAsync(QualityStandardSearchRequest request);
    }
}
