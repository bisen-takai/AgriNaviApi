using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Fields;
using AgriNaviApi.Application.Responses.Fields;
using AgriNaviApi.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FieldsController : ControllerBase
    {
        private readonly IFieldService _fieldService;

        public FieldsController(IFieldService fieldService)
        {
            _fieldService = fieldService;
        }

        /// <summary>
        /// 圃場テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<FieldCreateResponse>> CreateField(
            [FromBody] FieldCreateRequest request,
            CancellationToken cancellationToken)
        {
            var createdField = await _fieldService.CreateFieldAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetFieldById), new { id = createdField.Id }, createdField);
        }

        /// <summary>
        /// 圃場テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id">圃場ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<FieldDetailResponse>> GetFieldById(
            int id,
            CancellationToken cancellationToken)
        {
            var field = await _fieldService.GetFieldByIdAsync(id, cancellationToken);
            return Ok(field);
        }

        /// <summary>
        /// 圃場テーブルを更新する
        /// </summary>
        /// <param name="id">圃場ID</param>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<FieldUpdateResponse>> UpdateField(
            int id,
            [FromBody] FieldUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var updatedField = await _fieldService.UpdateFieldAsync(id, request, cancellationToken);
            return Ok(updatedField);
        }

        /// <summary>
        /// 圃場テーブルから削除する
        /// </summary>
        /// <param name="id">圃場ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteResponse>> DeleteField(
            int id,
            CancellationToken cancellationToken)
        {
            var deletedResult = await _fieldService.DeleteFieldAsync(id, cancellationToken);
            return Ok(deletedResult);
        }

        /// <summary>
        /// 圃場テーブルを検索する
        /// </summary>
        /// <param name="request">検索用リクエスト（ページング・ソート含む）</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<SearchResponse<FieldListItemResponse>>> SearchFields(
            [FromQuery] FieldSearchRequest request,
            CancellationToken cancellationToken)
        {
            var searchResult = await _fieldService.SearchFieldAsync(request, cancellationToken);
            return Ok(searchResult);
        }
    }
}
