using AgriNaviApi.Application.DTOs.Users;
using AgriNaviApi.Application.Requests.Users;
using AgriNaviApi.Common.Exceptions;

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
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DuplicateEntityException"></exception>
        public Task<UserCreateDto> CreateUserAsync(UserCreateRequest request);

        /// <summary>
        /// ユーザテーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public Task<UserDetailDto> GetUserByIdAsync(int id);

        /// <summary>
        /// ユーザテーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public Task<UserUpdateDto> UpdateUserAsync(UserUpdateRequest request);

        /// <summary>
        /// ユーザパスワードを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public Task<UserUpdateDto> UpdateUserPasswordAsync(PasswordUpdateRequest request);

        /// <summary>
        /// ユーザテーブルから削除する※削除フラグをtrueにするのみで、実際は削除しない
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Task<UserDeleteDto> DeleteUserAsync(UserDeleteRequest request);

        /// <summary>
        /// ユーザテーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<UserSearchDto> SearchUserAsync(UserSearchRequest request);

        /// <summary>
        /// ログインを行う
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Task<UserLoginDto> AuthenticateAsync(UserLoginRequest request);
    }
}
