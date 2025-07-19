using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.ShipmentLines;
using AgriNaviApi.Application.Responses.ShipmentLines;
using AgriNaviApi.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShipmentLinesController : ControllerBase
    {
        private readonly IShipmentLineService _shipmentLineService;

        public ShipmentLinesController(IShipmentLineService shipmentLineService)
        {
            _shipmentLineService = shipmentLineService;
        }

        /// <summary>
        /// 出荷記録詳細テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ShipmentLineCreateResponse>> CreateShipmentLine(
            [FromBody] ShipmentLineCreateRequest request,
            CancellationToken cancellationToken)
        {
            var createdShipmentLine = await _shipmentLineService.CreateShipmentLineAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetShipmentLineById), new { id = createdShipmentLine.Uuid }, createdShipmentLine);
        }

        /// <summary>
        /// 出荷記録詳細テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id">出荷記録詳細ID</param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ShipmentLineDetailResponse>> GetShipmentLineById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var shipmentLine = await _shipmentLineService.GetShipmentLineByIdAsync(id, cancellationToken);
            return Ok(shipmentLine);
        }

        /// <summary>
        /// 出荷記録詳細テーブルを更新する
        /// </summary>
        /// <param name="id">出荷記録詳細ID</param>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ShipmentLineUpdateResponse>> UpdateShipmentLine(
            Guid id,
            [FromBody] ShipmentLineUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var updatedShipmentLine = await _shipmentLineService.UpdateShipmentLineAsync(id, request, cancellationToken);
            return Ok(updatedShipmentLine);
        }

        /// <summary>
        /// 出荷記録詳細テーブルから削除する
        /// </summary>
        /// <param name="id">出荷記録詳細ID</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<DeleteWithUuidResponse>> DeleteShipmentLine(
            Guid id,
            CancellationToken cancellationToken)
        {
            var deletedResult = await _shipmentLineService.DeleteShipmentLineAsync(id, cancellationToken);
            return Ok(deletedResult);
        }

        /// <summary>
        /// 出荷記録詳細テーブルを検索する
        /// </summary>
        /// <param name="request">検索用リクエスト（ページング・ソート含む）</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<SearchResponse<ShipmentLineListItemResponse>>> SearchShipmentLines(
            [FromQuery] ShipmentLineSearchRequest request,
            CancellationToken cancellationToken)
        {
            var searchResult = await _shipmentLineService.SearchShipmentLineAsync(request, cancellationToken);
            return Ok(searchResult);
        }
    }
}
