using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.QualityStandards;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.QualityStandards;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class QualityStandardsController : ControllerBase
    {
        private readonly IQualityStandardService _qualityStandardService;

        public QualityStandardsController(IQualityStandardService qualityStandardService)
        {
            _qualityStandardService = qualityStandardService;
        }

        /// <summary>
        /// 品質・規格テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<QualityStandardCreateResponse>> CreateQualityStandard(
            [FromBody] QualityStandardCreateRequest request,
            CancellationToken cancellationToken)
        {
            var createdQualityStandard = await _qualityStandardService.CreateQualityStandardAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetQualityStandardById), new { id = createdQualityStandard.Id }, createdQualityStandard);
        }

        /// <summary>
        /// 品質・規格テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id">品質・規格ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<QualityStandardDetailResponse>> GetQualityStandardById(
            int id,
            CancellationToken cancellationToken)
        {
            var qualityStandard = await _qualityStandardService.GetQualityStandardByIdAsync(id, cancellationToken);
            return Ok(qualityStandard);
        }

        /// <summary>
        /// 品質・規格テーブルを更新する
        /// </summary>
        /// <param name="id">品質・規格ID</param>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<QualityStandardUpdateResponse>> UpdateQualityStandard(
            int id,
            [FromBody] QualityStandardUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var updatedQualityStandard = await _qualityStandardService.UpdateQualityStandardAsync(id, request, cancellationToken);
            return Ok(updatedQualityStandard);
        }

        /// <summary>
        /// 品質・規格テーブルから削除する
        /// </summary>
        /// <param name="id">品質・規格ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteResponse>> DeleteQualityStandard(
            int id,
            CancellationToken cancellationToken)
        {
            var deletedResult = await _qualityStandardService.DeleteQualityStandardAsync(id, cancellationToken);
            return Ok(deletedResult);
        }

        /// <summary>
        /// 品質・規格テーブルを検索する
        /// </summary>
        /// <param name="request">検索用リクエスト（ページング・ソート含む）</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<SearchResponse<QualityStandardListItemResponse>>> SearchQualityStandards(
            [FromQuery] QualityStandardSearchRequest request,
            CancellationToken cancellationToken)
        {
            var searchResult = await _qualityStandardService.SearchQualityStandardAsync(request, cancellationToken);
            return Ok(searchResult);
        }
    }
}
