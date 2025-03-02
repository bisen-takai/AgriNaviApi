using AgriNaviApi.Application.DTOs.QualityStandards;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.QualityStandards;
using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class QualityStandardController : Controller
    {
        private readonly IQualityStandardService _qualityStandardService;

        public QualityStandardController(IQualityStandardService qualityStandardService)
        {
            _qualityStandardService = qualityStandardService;
        }

        /// <summary>
        /// 品質・規格テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<QualityStandardCreateDto>> CreateQualityStandard([FromBody] QualityStandardCreateRequest request)
        {
            try
            {
                var createdQualityStandard = await _qualityStandardService.CreateQualityStandardAsync(request);
                return CreatedAtAction(nameof(GetQualityStandardById), new { id = createdQualityStandard.Id }, createdQualityStandard);
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
        /// 品質・規格テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<QualityStandardDetailDto>> GetQualityStandardById(int id)
        {
            try
            {
                var createdQualityStandard = await _qualityStandardService.GetQualityStandardByIdAsync(id);
                return Ok(createdQualityStandard);

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
        /// 品質・規格テーブルを更新する
        /// </summary>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<QualityStandardUpdateDto>> UpdateQualityStandard([FromBody] QualityStandardUpdateRequest request)
        {
            try
            {
                var updatedQualityStandard = await _qualityStandardService.UpdateQualityStandardAsync(request);
                return Ok(updatedQualityStandard);
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
        /// 品質・規格テーブルから削除する
        /// </summary>
        /// <param name="request">削除用リクエストデータ</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteQualityStandard([FromBody] QualityStandardDeleteRequest request)
        {
            try
            {
                var deletedResult = await _qualityStandardService.DeleteQualityStandardAsync(request);
                return Ok(deletedResult);
            }
            catch (InvalidOperationException ex)
            {
                // 対象品質・規格が見つからなかった場合などは NotFound を返す
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// 品質・規格テーブルを検索する
        /// </summary>
        /// <param name="searchQualityStandardName">品質・規格名</param>
        /// <param name="searchMatchType">検索の種類</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchQualityStandards(
            [FromQuery] string searchQualityStandardName,
            [FromQuery] SearchMatchType searchMatchType = SearchMatchType.None)
        {
            try
            {
                var request = new QualityStandardSearchRequest
                {
                    SearchQualityStandardName = searchQualityStandardName,
                    SearchMatchType = searchMatchType
                };

                var searchResult = await _qualityStandardService.SearchQualityStandardAsync(request);
                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
