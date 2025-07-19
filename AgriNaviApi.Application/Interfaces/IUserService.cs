using AgriNaviApi.Application.Requests.Users;
using AgriNaviApi.Application.Responses.Users;
using AgriNaviApi.Application.Responses;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// ユーザテーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// ユーザテーブルに登録する
        /// </summary>
        /// <param name="request">登録リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<UserCreateResponse> CreateUserAsync(
            UserCreateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// ユーザテーブル詳細を取得する
        /// </summary>
        /// <param name="id">ユーザID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<UserDetailResponse> GetUserByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        /// <summary>
        /// ユーザテーブルを更新する
        /// </summary>
        /// <param name="id">ユーザID</param>
        /// <param name="request">更新リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<UserUpdateResponse> UpdateUserAsync(
            Guid id,
            UserUpdateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// ユーザパスワードを更新する
        /// </summary>
        /// <param name="request">パスワード更新リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<PasswordUpdateResponse> UpdateUserPasswordAsync(
            Guid id,
            PasswordUpdateRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// ユーザテーブルから削除する
        /// </summary>
        /// <param name="id">ユーザID</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<DeleteWithUuidResponse> DeleteUserAsync(
            Guid id,
            CancellationToken cancellationToken);

        /// <summary>
        /// ユーザテーブルを検索する
        /// </summary>
        /// <param name="request">検索リクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<SearchResponse<UserListItemResponse>> SearchUserAsync(
            UserSearchRequest request,
            CancellationToken cancellationToken);

        /// <summary>
        /// ログインを行う
        /// </summary>
        /// <param name="request">ログインリクエスト</param>
        /// <param name="cancellationToken">操作をキャンセルするトークン</param>
        /// <returns></returns>
        Task<UserLoginResponse> AuthenticateAsync(
            UserLoginRequest request,
            CancellationToken cancellationToken);
    }
}
