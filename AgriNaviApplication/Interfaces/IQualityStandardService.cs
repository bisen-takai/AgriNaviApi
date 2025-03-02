using AgriNaviApi.Application.DTOs.QualityStandards;
using AgriNaviApi.Application.Requests.QualityStandards;

namespace AgriNaviApi.Application.Interfaces
{
    public interface IQualityStandardService
    {
        /// <summary>
        /// 単位テーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<QualityStandardCreateDto> CreateQualityStandardAsync(QualityStandardCreateRequest request);

        /// <summary>
        /// 単位テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<QualityStandardDetailDto> GetQualityStandardByIdAsync(int id);

        /// <summary>
        /// 単位テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<QualityStandardUpdateDto> UpdateQualityStandardAsync(QualityStandardUpdateRequest request);

        /// <summary>
        /// 単位テーブルから削除する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<QualityStandardDeleteDto> DeleteQualityStandardAsync(QualityStandardDeleteRequest request);

        /// <summary>
        /// 単位テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<QualityStandardSearchDto> SearchQualityStandardAsync(QualityStandardSearchRequest request);
    }
}
