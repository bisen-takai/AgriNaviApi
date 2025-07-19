using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Users;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.Users;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// ユーザテーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<UserCreateResponse>> CreateUser(
            [FromBody] UserCreateRequest request,
            CancellationToken cancellationToken)
        {
            var createdUser = await _userService.CreateUserAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Uuid }, createdUser);
        }

        /// <summary>
        /// ユーザテーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id">ユーザUUID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailResponse>> GetUserById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var User = await _userService.GetUserByIdAsync(id, cancellationToken);
            return Ok(User);
        }

        /// <summary>
        /// ユーザテーブルを更新する
        /// </summary>
        /// <param name="id">ユーザUUID</param>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserUpdateResponse>> UpdateUser(
            Guid id,
            [FromBody] UserUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var updatedUser = await _userService.UpdateUserAsync(id, request, cancellationToken);
            return Ok(updatedUser);
        }

        /// <summary>
        /// ユーザのパスワードを更新する
        /// </summary>
        /// <param name="id">ユーザUUID</param>
        /// <param name="request">パスワード更新用リクエストデータ</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}/password")]
        public async Task<ActionResult<PasswordUpdateResponse>> UpdateUserPassword(
            Guid id,
            [FromBody] PasswordUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var updatedUser = await _userService.UpdateUserPasswordAsync(id, request, cancellationToken);
            return Ok(updatedUser);
        }

        /// <summary>
        /// ユーザテーブルから削除する
        /// </summary>
        /// <param name="id">ユーザID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteWithUuidResponse>> DeleteUser(
            Guid id,
            CancellationToken cancellationToken)
        {
            var deletedResult = await _userService.DeleteUserAsync(id, cancellationToken);
            return Ok(deletedResult);
        }

        /// <summary>
        /// ユーザテーブルを検索する
        /// </summary>
        /// <param name="request">検索用リクエスト（ページング・ソート含む）</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<SearchResponse<UserListItemResponse>>> SearchUsers(
            [FromQuery] UserSearchRequest request,
            CancellationToken cancellationToken)
        {
            var searchResult = await _userService.SearchUserAsync(request, cancellationToken);
            return Ok(searchResult);
        }

        /// <summary>
        /// ユーザログインを行う
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<ActionResult<UserLoginResponse>> Login(
            [FromBody] UserLoginRequest request,
            CancellationToken cancellationToken)
        {
            var createdUser = await _userService.AuthenticateAsync(request, cancellationToken);
            return Ok(createdUser);
        }
    }
}
