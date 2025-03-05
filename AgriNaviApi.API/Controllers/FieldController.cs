using AgriNaviApi.Application.DTOs.Fields;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Fields;
using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class FieldController : Controller
    {
        private readonly IFieldService _fieldService;

        public FieldController(IFieldService fieldService)
        {
            _fieldService = fieldService;
        }

        /// <summary>
        /// 圃場テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<FieldCreateDto>> CreateField([FromBody] FieldCreateRequest request)
        {
            try
            {
                var createdField = await _fieldService.CreateFieldAsync(request);
                return CreatedAtAction(nameof(GetFieldById), new { id = createdField.Id }, createdField);
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
        /// 圃場テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<FieldDetailDto>> GetFieldById(int id)
        {
            try
            {
                var createdField = await _fieldService.GetFieldByIdAsync(id);
                return Ok(createdField);

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
        /// 圃場テーブルを更新する
        /// </summary>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<FieldUpdateDto>> UpdateField([FromBody] FieldUpdateRequest request)
        {
            try
            {
                var updatedField = await _fieldService.UpdateFieldAsync(request);
                return Ok(updatedField);
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
        /// 圃場テーブルから削除する
        /// </summary>
        /// <param name="request">削除用リクエストデータ</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteField([FromBody] FieldDeleteRequest request)
        {
            try
            {
                var deletedResult = await _fieldService.DeleteFieldAsync(request);
                return Ok(deletedResult);
            }
            catch (InvalidOperationException ex)
            {
                // 対象圃場が見つからなかった場合などは NotFound を返す
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// 圃場テーブルを検索する
        /// </summary>
        /// <param name="searchFieldName">圃場名</param>
        /// <param name="searchMatchType">検索の種類</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchFields(
            [FromQuery] string searchFieldName,
            [FromQuery] SearchMatchType searchMatchType = SearchMatchType.None)
        {
            try
            {
                var request = new FieldSearchRequest
                {
                    SearchFieldName = searchFieldName,
                    SearchMatchType = searchMatchType
                };

                var searchResult = await _fieldService.SearchFieldAsync(request);
                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
