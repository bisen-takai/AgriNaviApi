using AgriNaviApi.Application.DTOs.SeasonCropSchedules;
using AgriNaviApi.Application.Requests.SeasonCropSchedules;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 作付計画テーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface ISeasonCropScheduleService
    {
        /// <summary>
        /// 作付計画テーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<SeasonCropScheduleCreateDto> CreateSeasonCropScheduleAsync(SeasonCropScheduleCreateRequest request);

        /// <summary>
        /// 作付計画テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<SeasonCropScheduleDetailDto> GetSeasonCropScheduleByIdAsync(int id);

        /// <summary>
        /// 作付計画テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<SeasonCropScheduleUpdateDto> UpdateSeasonCropScheduleAsync(SeasonCropScheduleUpdateRequest request);

        /// <summary>
        /// 作付計画テーブルから削除する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<SeasonCropScheduleDeleteDto> DeleteSeasonCropScheduleAsync(SeasonCropScheduleDeleteRequest request);

        /// <summary>
        /// 作付計画テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<SeasonCropScheduleSearchDto> SearchSeasonCropScheduleAsync(SeasonCropScheduleSearchRequest request);
    }
}
