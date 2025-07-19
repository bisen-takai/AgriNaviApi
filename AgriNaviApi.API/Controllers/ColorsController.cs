using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Colors;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.Colors;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IColorService _colorService;

        public ColorsController(IColorService colorService)
        {
            _colorService = colorService;
        }

        /// <summary>
        /// カラーテーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ColorCreateResponse>> CreateColor(
            [FromBody] ColorCreateRequest request,
            CancellationToken cancellationToken)
        {
            var createdColor = await _colorService.CreateColorAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetColorById), new { id = createdColor.Id }, createdColor);
        }

        /// <summary>
        /// カラーテーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ColorDetailResponse>> GetColorById(
            int id,
            CancellationToken cancellationToken)
        {
            var color = await _colorService.GetColorByIdAsync(id, cancellationToken);
            return Ok(color);
        }

        /// <summary>
        /// カラーテーブルを更新する
        /// </summary>
        /// <param name="id">カラーID</param>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ColorUpdateResponse>> UpdateColor(
            int id,
            [FromBody] ColorUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var updatedColor = await _colorService.UpdateColorAsync(id, request, cancellationToken);
            return Ok(updatedColor);
        }

        /// <summary>
        /// カラーテーブルから削除する
        /// </summary>
        /// <param name="id">削除対象のカラーID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteResponse>> DeleteColor(
            int id,
            CancellationToken cancellationToken)
        {
            var deletedResult = await _colorService.DeleteColorAsync(id, cancellationToken);
            return Ok(deletedResult);
        }

        /// <summary>
        /// カラーテーブルを検索する（ページング／ソート対応）
        /// </summary>
        /// <param name="request">検索用リクエスト（ページング・ソート含む）</param>
        /// <returns>検索結果＋総件数＋ページ情報</returns>
        [HttpGet]
        public async Task<ActionResult<SearchResponse<ColorListItemResponse>>> SearchColors(
            [FromQuery] ColorSearchRequest request,
            CancellationToken cancellationToken)
        {
            var searchResult = await _colorService.SearchColorAsync(request, cancellationToken);
            return Ok(searchResult);
        }
    }
}
