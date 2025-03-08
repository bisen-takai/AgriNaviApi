using AgriNaviApi.Application.DTOs.Users;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Users;
using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// ユーザテーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UserCreateDto>> CreateUser([FromBody] UserCreateRequest request)
        {
            try
            {
                var createdUser = await _userService.CreateUserAsync(request);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (DuplicateEntityException ex)
            {
                // 重複エラーの場合は、Conflictを返す
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// ユーザログインを行う
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UserLoginDto>> Login([FromBody] UserLoginRequest request)
        {
            try
            {
                var createdUser = await _userService.AuthenticateAsync(request);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (DuplicateEntityException ex)
            {
                // 重複エラーの場合は、Conflictを返す
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// ユーザテーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailDto>> GetUserById(int id)
        {
            try
            {
                var createdUser = await _userService.GetUserByIdAsync(id);
                return Ok(createdUser);

            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// ユーザテーブルを更新する
        /// </summary>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<UserUpdateDto>> UpdateUser([FromBody] UserUpdateRequest request)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(request);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// ユーザテーブルから削除する
        /// </summary>
        /// <param name="request">削除用リクエストデータ</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] UserDeleteRequest request)
        {
            try
            {
                var deletedResult = await _userService.DeleteUserAsync(request);
                return Ok(deletedResult);
            }
            catch (InvalidOperationException ex)
            {
                // 対象ユーザが見つからなかった場合などは NotFound を返す
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// ユーザテーブルを検索する
        /// </summary>
        /// <param name="searchUserName">ユーザ</param>
        /// <param name="searchMatchType">検索の種類</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchUsers(
            [FromQuery] string searchUserName,
            [FromQuery] SearchMatchType searchMatchType = SearchMatchType.None)
        {
            try
            {
                var request = new UserSearchRequest
                {
                    SearchUserName = searchUserName,
                    SearchMatchType = searchMatchType
                };

                var searchResult = await _userService.SearchUserAsync(request);
                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
