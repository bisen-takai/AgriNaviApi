using AgriNaviApi.Application.Requests.Groups;
using AgriNaviApi.Application.Responses.Groups;
using AgriNaviApi.Application.Responses;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// グループテーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IGroupService
    {
        /// <summary>
        /// グループテーブルに登録する
        /// </summary>
        /// <param name="request">登録リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<GroupCreateResponse> CreateGroupAsync(
            GroupCreateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// グループテーブル詳細を取得する
        /// </summary>
        /// <param name="id">グループID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<GroupDetailResponse> GetGroupByIdAsync(
            int id,
            CancellationToken cancellationToken);

        /// <summary>
        /// グループテーブルを更新する
        /// </summary>
        /// <param name="id">グループID</param>
        /// <param name="request">更新リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<GroupUpdateResponse> UpdateGroupAsync(
            int id,
            GroupUpdateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// グループテーブルから削除する
        /// </summary>
        /// <param name="id">グループID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<DeleteResponse> DeleteGroupAsync(
            int id,
            CancellationToken cancellationToken);

        /// <summary>
        /// グループテーブルを検索する
        /// </summary>
        /// <param name="request">検索リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<SearchResponse<GroupListItemResponse>> SearchGroupAsync(
            GroupSearchRequest request,
            CancellationToken cancellationToken);
    }
}
