using AgriNaviApi.Application.Requests.SeasonSchedules;
using AgriNaviApi.Application.Responses.SeasonSchedules;
using AgriNaviApi.Application.Responses;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 作付計画テーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface ISeasonScheduleService
    {
        /// <summary>
        /// 作付計画テーブルに登録する
        /// </summary>
        /// <param name="request">登録リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<SeasonScheduleCreateResponse> CreateSeasonScheduleAsync(
            SeasonScheduleCreateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 作付計画テーブル詳細を取得する
        /// </summary>
        /// <param name="id">作付計画ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<SeasonScheduleDetailResponse> GetSeasonScheduleByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 作付計画テーブルを更新する
        /// </summary>
        /// <param name="id">作付計画ID</param>
        /// <param name="request">更新リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<SeasonScheduleUpdateResponse> UpdateSeasonScheduleAsync(
            Guid id,
            SeasonScheduleUpdateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// 作付計画テーブルから削除する
        /// </summary>
        /// <param name="id">作付計画ID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<DeleteWithUuidResponse> DeleteSeasonScheduleAsync(
            Guid id,
            CancellationToken cancellationToken);

        /// <summary>
        /// 作付計画テーブルを検索する
        /// </summary>
        /// <param name="request">検索リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<SearchResponse<SeasonScheduleListItemResponse>> SearchSeasonScheduleAsync(
            SeasonScheduleSearchRequest request,
            CancellationToken cancellationToken);
    }
}
