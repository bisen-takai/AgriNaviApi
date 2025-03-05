using AgriNaviApi.Application.DTOs.Crops;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Crops;
using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class CropController : Controller
    {
        private readonly ICropService _cropService;

        public CropController(ICropService cropService)
        {
            _cropService = cropService;
        }

        /// <summary>
        /// 作付名テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CropCreateDto>> CreateCrop([FromBody] CropCreateRequest request)
        {
            try
            {
                var createdCrop = await _cropService.CreateCropAsync(request);
                return CreatedAtAction(nameof(GetCropById), new { id = createdCrop.Id }, createdCrop);
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
        /// 作付名テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CropDetailDto>> GetCropById(int id)
        {
            try
            {
                var createdCrop = await _cropService.GetCropByIdAsync(id);
                return Ok(createdCrop);

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
        /// 作付名テーブルを更新する
        /// </summary>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<CropUpdateDto>> UpdateCrop([FromBody] CropUpdateRequest request)
        {
            try
            {
                var updatedCrop = await _cropService.UpdateCropAsync(request);
                return Ok(updatedCrop);
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
        /// 作付名テーブルから削除する
        /// </summary>
        /// <param name="request">削除用リクエストデータ</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteCrop([FromBody] CropDeleteRequest request)
        {
            try
            {
                var deletedResult = await _cropService.DeleteCropAsync(request);
                return Ok(deletedResult);
            }
            catch (InvalidOperationException ex)
            {
                // 対象作付名が見つからなかった場合などは NotFound を返す
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// 作付名テーブルを検索する
        /// </summary>
        /// <param name="searchCropName">作付名</param>
        /// <param name="searchMatchType">検索の種類</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchCrops(
            [FromQuery] string searchCropName,
            [FromQuery] SearchMatchType searchMatchType = SearchMatchType.None)
        {
            try
            {
                var request = new CropSearchRequest
                {
                    SearchCropName = searchCropName,
                    SearchMatchType = searchMatchType
                };

                var searchResult = await _cropService.SearchCropAsync(request);
                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
