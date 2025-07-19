using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Units;
using AgriNaviApi.Application.Responses.Units;
using AgriNaviApi.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitsController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        /// <summary>
        /// 単位テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UnitCreateResponse>> CreateUnit(
            [FromBody] UnitCreateRequest request,
            CancellationToken cancellationToken)
        {
            var createdUnit = await _unitService.CreateUnitAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetUnitById), new { id = createdUnit.Id }, createdUnit);
        }

        /// <summary>
        /// 単位テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id">単位ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UnitDetailResponse>> GetUnitById(
            int id,
            CancellationToken cancellationToken)
        {
            var unit = await _unitService.GetUnitByIdAsync(id, cancellationToken);
            return Ok(unit);
        }

        /// <summary>
        /// 単位テーブルを更新する
        /// </summary>
        /// <param name="id">単位ID</param>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UnitUpdateResponse>> UpdateUnit(
            int id,
            [FromBody] UnitUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var updatedUnit = await _unitService.UpdateUnitAsync(id, request, cancellationToken);
            return Ok(updatedUnit);
        }

        /// <summary>
        /// 単位テーブルから削除する
        /// </summary>
        /// <param name="id">単位ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteResponse>> DeleteUnit(
            int id,
            CancellationToken cancellationToken)
        {
            var deletedResult = await _unitService.DeleteUnitAsync(id, cancellationToken);
            return Ok(deletedResult);
        }

        /// <summary>
        /// 単位テーブルを検索する
        /// </summary>
        /// <param name="request">検索用リクエスト（ページング・ソート含む）</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<SearchResponse<UnitListItemResponse>>> SearchUnits(
            [FromQuery] UnitSearchRequest request,
            CancellationToken cancellationToken)
        {
            var searchResult = await _unitService.SearchUnitAsync(request, cancellationToken);
            return Ok(searchResult);
        }
    }
}
