using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Crops;
using AgriNaviApi.Application.Responses.Crops;
using AgriNaviApi.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CropsController : ControllerBase
    {
        private readonly ICropService _cropService;

        public CropsController(ICropService cropService)
        {
            _cropService = cropService;
        }

        /// <summary>
        /// 作付テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CropCreateResponse>> CreateCrop(
            [FromBody] CropCreateRequest request,
            CancellationToken cancellationToken)
        {
            var createdCrop = await _cropService.CreateCropAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetCropById), new { id = createdCrop.Id }, createdCrop);
        }

        /// <summary>
        /// 作付テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id">作付ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CropDetailResponse>> GetCropById(
            int id,
            CancellationToken cancellationToken)
        {
            var crop = await _cropService.GetCropByIdAsync(id, cancellationToken);
            return Ok(crop);
        }

        /// <summary>
        /// 作付テーブルを更新する
        /// </summary>
        /// <param name="id">作付ID</param>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<CropUpdateResponse>> UpdateCrop(
            int id,
            [FromBody] CropUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var updatedCrop = await _cropService.UpdateCropAsync(id, request, cancellationToken);
            return Ok(updatedCrop);
        }

        /// <summary>
        /// 作付テーブルから削除する
        /// </summary>
        /// <param name="id">作付ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteResponse>> DeleteCrop(
            int id,
            CancellationToken cancellationToken)
        {
            var deletedResult = await _cropService.DeleteCropAsync(id, cancellationToken);
            return Ok(deletedResult);
        }

        /// <summary>
        /// 作付テーブルを検索する
        /// </summary>
        /// <param name="request">検索用リクエスト（ページング・ソート含む）</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<SearchResponse<CropListItemResponse>>> SearchCrops(
            [FromQuery] CropSearchRequest request,
            CancellationToken cancellationToken)
        {
            var searchResult = await _cropService.SearchCropAsync(request, cancellationToken);
            return Ok(searchResult);
        }
    }
}
