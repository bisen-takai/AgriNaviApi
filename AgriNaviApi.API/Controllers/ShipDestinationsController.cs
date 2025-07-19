using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.ShipDestinations;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.ShipDestinations;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShipDestinationsController : ControllerBase
    {
        private readonly IShipDestinationService _shipDestinationService;

        public ShipDestinationsController(IShipDestinationService shipDestinationService)
        {
            _shipDestinationService = shipDestinationService;
        }

        /// <summary>
        /// 出荷先テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ShipDestinationCreateResponse>> CreateShipDestination(
            [FromBody] ShipDestinationCreateRequest request,
            CancellationToken cancellationToken)
        {
            var createdShipDestination = await _shipDestinationService.CreateShipDestinationAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetShipDestinationById), new { id = createdShipDestination.Id }, createdShipDestination);
        }

        /// <summary>
        /// 出荷先テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id">出荷先ID</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ShipDestinationDetailResponse>> GetShipDestinationById(
            int id,
            CancellationToken cancellationToken)
        {
            var shipDestination = await _shipDestinationService.GetShipDestinationByIdAsync(id, cancellationToken);
            return Ok(shipDestination);
        }

        /// <summary>
        /// 出荷先テーブルを更新する
        /// </summary>
        /// <param name="id">出荷先ID</param>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ShipDestinationUpdateResponse>> UpdateShipDestination(
            int id,
            [FromBody] ShipDestinationUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var updatedShipDestination = await _shipDestinationService.UpdateShipDestinationAsync(id, request, cancellationToken);
            return Ok(updatedShipDestination);
        }

        /// <summary>
        /// 出荷先テーブルから削除する
        /// </summary>
        /// <param name="id">出荷先ID</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteResponse>> DeleteShipDestination(
            int id,
            CancellationToken cancellationToken)
        {
            var deletedResult = await _shipDestinationService.DeleteShipDestinationAsync(id, cancellationToken);
            return Ok(deletedResult);
        }

        /// <summary>
        /// 出荷先テーブルを検索する
        /// </summary>
        /// <param name="request">検索用リクエスト（ページング・ソート含む）</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<SearchResponse<ShipDestinationListItemResponse>>> SearchShipDestinations(
            [FromQuery] ShipDestinationSearchRequest request,
            CancellationToken cancellationToken)
        {
            var searchResult = await _shipDestinationService.SearchShipDestinationAsync(request, cancellationToken);
            return Ok(searchResult);
        }
    }
}
