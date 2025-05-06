using AgriNaviApi.Application.DTOs.Colors;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Colors;
using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ColorController : Controller
    {
        private readonly IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        /// <summary>
        /// カラーテーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ColorCreateDto>> CreateColor([FromBody] ColorCreateRequest request)
        {
            try
            {
                var createdColor = await _colorService.CreateColorAsync(request);
                return CreatedAtAction(nameof(GetColorById), new { id = createdColor.Id }, createdColor);
            }
            catch (DuplicateEntityException ex)
            {
                // 重複エラーの場合は、Conflictを返す
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new {message = ex.Message});
            }
        }

        /// <summary>
        /// カラーテーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ColorDetailDto>> GetColorById(int id)
        {
            try
            {
                var createdColor = await _colorService.GetColorByIdAsync(id);
                return Ok(createdColor);

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
        /// カラーテーブルを更新する
        /// </summary>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ColorUpdateDto>> UpdateColor(int id, [FromBody] ColorUpdateRequest request)
        {
            try
            {
                var updatedColor = await _colorService.UpdateColorAsync(request);
                return Ok(updatedColor);
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
        /// カラーテーブルから削除する
        /// </summary>
        /// <param name="request">削除用リクエストデータ</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColor(int id)
        {
            try
            {
                var deletedResult = await _colorService.DeleteColorAsync(id);
                return Ok(deletedResult);
            }
            catch (InvalidOperationException ex)
            {
                // 対象カラーが見つからなかった場合などは NotFound を返す
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// カラーテーブルを検索する
        /// </summary>
        /// <param name="searchColorName">カラー名</param>
        /// <param name="searchMatchType">検索の種類</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchColors(
            [FromQuery] string? searchColorName,
            [FromQuery] SearchMatchType searchMatchType = SearchMatchType.None)
        {
            try
            {
                var request = new ColorSearchRequest
                {
                    SearchColorName = searchColorName,
                    SearchMatchType = searchMatchType
                };

                var searchResult = await _colorService.SearchColorAsync(request);
                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
