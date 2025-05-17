using AgriNaviApi.Application.DTOs.Units;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Units;
using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class UnitController : Controller
    {
        private readonly IUnitService _unitService;

        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        /// <summary>
        /// 単位テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<UnitCreateDto>> CreateUnit([FromBody] UnitCreateRequest request)
        {
            try
            {
                var createdUnit = await _unitService.CreateUnitAsync(request);
                return CreatedAtAction(nameof(GetUnitById), new { id = createdUnit.Id }, createdUnit);
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
        /// 単位テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<UnitDetailDto>> GetUnitById(int id)
        {
            try
            {
                var createdUnit = await _unitService.GetUnitByIdAsync(id);
                return Ok(createdUnit);

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
        /// 単位テーブルを更新する
        /// </summary>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<UnitUpdateDto>> UpdateUnit(int id, [FromBody] UnitUpdateRequest request)
        {
            try
            {
                var updatedUnit = await _unitService.UpdateUnitAsync(request);
                return Ok(updatedUnit);
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
        /// 単位テーブルから削除する
        /// </summary>
        /// <param name="request">削除用リクエストデータ</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnit(int id)
        {
            try
            {
                var deletedResult = await _unitService.DeleteUnitAsync(id);
                return Ok(deletedResult);
            }
            catch (InvalidOperationException ex)
            {
                // 対象単位が見つからなかった場合などは NotFound を返す
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// 単位テーブルを検索する
        /// </summary>
        /// <param name="searchUnitName">単位名</param>
        /// <param name="searchMatchType">検索の種類</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchUnits(
            [FromQuery] string? searchUnitName,
            [FromQuery] SearchMatchType searchMatchType = SearchMatchType.None)
        {
            try
            {
                var request = new UnitSearchRequest
                {
                    SearchUnitName = searchUnitName,
                    SearchMatchType = searchMatchType
                };

                var searchResult = await _unitService.SearchUnitAsync(request);
                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
