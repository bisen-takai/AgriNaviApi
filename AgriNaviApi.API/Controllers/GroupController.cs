using AgriNaviApi.Application.DTOs.Groups;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Groups;
using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        /// <summary>
        /// グループテーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<GroupCreateDto>> CreateGroup([FromBody] GroupCreateRequest request)
        {
            try
            {
                var createdGroup = await _groupService.CreateGroupAsync(request);
                return CreatedAtAction(nameof(GetGroupById), new { id = createdGroup.Id }, createdGroup);
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
        /// グループテーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDetailDto>> GetGroupById(int id)
        {
            try
            {
                var createdGroup = await _groupService.GetGroupByIdAsync(id);
                return Ok(createdGroup);

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
        /// グループテーブルを更新する
        /// </summary>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<GroupUpdateDto>> UpdateGroup(int id, [FromBody] GroupUpdateRequest request)
        {
            try
            {
                var updatedGroup = await _groupService.UpdateGroupAsync(request);
                return Ok(updatedGroup);
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
        /// グループテーブルから削除する
        /// </summary>
        /// <param name="request">削除用リクエストデータ</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            try
            {
                var deletedResult = await _groupService.DeleteGroupAsync(id);
                return Ok(deletedResult);
            }
            catch (InvalidOperationException ex)
            {
                // 対象グループが見つからなかった場合などは NotFound を返す
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// グループテーブルを検索する
        /// </summary>
        /// <param name="searchGroupName">グループ名</param>
        /// <param name="searchMatchType">検索の種類</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchGroups(
            [FromQuery] string? searchGroupName,
            [FromQuery] GroupKind? groupKind,
            [FromQuery] SearchMatchType searchMatchType = SearchMatchType.None)
        {
            try
            {
                var request = new GroupSearchRequest
                {
                    SearchGroupName = searchGroupName,
                    Kind = groupKind,
                    SearchMatchType = searchMatchType
                };

                var searchResult = await _groupService.SearchGroupAsync(request);
                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
