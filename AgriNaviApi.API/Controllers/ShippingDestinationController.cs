using AgriNaviApi.Application.DTOs.ShippingDestinations;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.ShippingDestinations;
using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ShippingDestinationController : Controller
    {
        private readonly IShippingDestinationService _shippingDestinationService;

        public ShippingDestinationController(IShippingDestinationService shippingDestinationService)
        {
            _shippingDestinationService = shippingDestinationService;
        }

        /// <summary>
        /// 出荷先名テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ShippingDestinationCreateDto>> CreateShippingDestination([FromBody] ShippingDestinationCreateRequest request)
        {
            try
            {
                var createdShippingDestination = await _shippingDestinationService.CreateShippingDestinationAsync(request);
                return CreatedAtAction(nameof(GetShippingDestinationById), new { id = createdShippingDestination.Id }, createdShippingDestination);
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
        /// 出荷先名テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ShippingDestinationDetailDto>> GetShippingDestinationById(int id)
        {
            try
            {
                var createdShippingDestination = await _shippingDestinationService.GetShippingDestinationByIdAsync(id);
                return Ok(createdShippingDestination);

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
        /// 出荷先名テーブルを更新する
        /// </summary>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ShippingDestinationUpdateDto>> UpdateShippingDestination(int id, [FromBody] ShippingDestinationUpdateRequest request)
        {
            try
            {
                var updatedShippingDestination = await _shippingDestinationService.UpdateShippingDestinationAsync(request);
                return Ok(updatedShippingDestination);
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
        /// 出荷先名テーブルから削除する
        /// </summary>
        /// <param name="request">削除用リクエストデータ</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShippingDestination(int id)
        {
            try
            {
                var deletedResult = await _shippingDestinationService.DeleteShippingDestinationAsync(id);
                return Ok(deletedResult);
            }
            catch (InvalidOperationException ex)
            {
                // 対象出荷先名が見つからなかった場合などは NotFound を返す
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// 出荷先名テーブルを検索する
        /// </summary>
        /// <param name="searchShippingDestinationName">出荷先名</param>
        /// <param name="searchMatchType">検索の種類</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchShippingDestinations(
            [FromQuery] string? searchShippingDestinationName,
            [FromQuery] SearchMatchType searchMatchType = SearchMatchType.None)
        {
            try
            {
                var request = new ShippingDestinationSearchRequest
                {
                    SearchShippingDestinationName = searchShippingDestinationName,
                    SearchMatchType = searchMatchType
                };

                var searchResult = await _shippingDestinationService.SearchShippingDestinationAsync(request);
                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
