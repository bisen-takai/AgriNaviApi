using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Groups;
using AgriNaviApi.Application.Responses.Groups;
using AgriNaviApi.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        /// <summary>
        /// グループテーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<GroupCreateResponse>> CreateGroup(
            [FromBody] GroupCreateRequest request,
            CancellationToken cancellationToken)
        {
            var createdGroup = await _groupService.CreateGroupAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetGroupById), new { id = createdGroup.Id }, createdGroup);
        }

        /// <summary>
        /// グループテーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDetailResponse>> GetGroupById(
            int id,
            CancellationToken cancellationToken)
        {
            var group = await _groupService.GetGroupByIdAsync(id, cancellationToken);
            return Ok(group);
        }

        /// <summary>
        /// グループテーブルを更新する
        /// </summary>
        /// <param name="id">グループID</param>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<GroupUpdateResponse>> UpdateGroup(
            int id,
            [FromBody] GroupUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var updatedGroup = await _groupService.UpdateGroupAsync(id, request, cancellationToken);
            return Ok(updatedGroup);
        }

        /// <summary>
        /// グループテーブルから削除する
        /// </summary>
        /// <param name="id">削除対象のグループID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteResponse>> DeleteGroup(
            int id,
            CancellationToken cancellationToken)
        {
            var deletedResult = await _groupService.DeleteGroupAsync(id, cancellationToken);
            return Ok(deletedResult);
        }

        /// <summary>
        /// グループテーブルを検索する（ページング／ソート対応）
        /// </summary>
        /// <param name="request">検索用リクエスト（ページング・ソート含む）</param>
        /// <returns>検索結果＋総件数＋ページ情報</returns>
        [HttpGet]
        public async Task<ActionResult<SearchResponse<GroupListItemResponse>>> SearchGroups(
            [FromQuery] GroupSearchRequest request,
            CancellationToken cancellationToken)
        {
            var searchResult = await _groupService.SearchGroupAsync(request, cancellationToken);
            return Ok(searchResult);
        }
    }
}
