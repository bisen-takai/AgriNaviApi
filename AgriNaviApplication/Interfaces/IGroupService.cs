using AgriNaviApi.Application.DTOs.Groups;
using AgriNaviApi.Application.Requests.Groups;

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
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<GroupCreateDto> CreateGroupAsync(GroupCreateRequest request);

        /// <summary>
        /// グループテーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<GroupDetailDto> GetGroupByIdAsync(int id);

        /// <summary>
        /// グループテーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<GroupUpdateDto> UpdateGroupAsync(GroupUpdateRequest request);

        /// <summary>
        /// グループテーブルから削除する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<GroupDeleteDto> DeleteGroupAsync(int id);

        /// <summary>
        /// グループテーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<GroupSearchDto> SearchGroupAsync(GroupSearchRequest request);
    }
}
