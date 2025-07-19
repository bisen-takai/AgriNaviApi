using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.ShipmentWithLines;
using AgriNaviApi.Application.Responses.ShipmentWithLines;
using AgriNaviApi.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShipmentWithLinesController : ControllerBase
    {
        private readonly IShipmentWithLineService _shipmentWithLineService;

        public ShipmentWithLinesController(IShipmentWithLineService shipmentWithLineService)
        {
            _shipmentWithLineService = shipmentWithLineService;
        }

        /// <summary>
        /// 出荷記録テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ShipmentWithLineCreateResponse>> CreateShipmentWithLine(
            [FromBody] ShipmentWithLineCreateRequest request,
            CancellationToken cancellationToken)
        {
            var createdShipmentWithLine = await _shipmentWithLineService.CreateShipmentWithLineAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetShipmentWithLineById), new { id = createdShipmentWithLine.Uuid }, createdShipmentWithLine);
        }

        /// <summary>
        /// 出荷記録テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ShipmentWithLineDetailResponse>> GetShipmentWithLineById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var shipmentWithLine = await _shipmentWithLineService.GetShipmentWithLineByIdAsync(id, cancellationToken);
            return Ok(shipmentWithLine);
        }

        /// <summary>
        /// 出荷記録テーブルを更新する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ShipmentWithLineUpdateResponse>> UpdateShipmentWithLine(
            Guid id,
            [FromBody] ShipmentWithLineUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var updatedShipmentWithLine = await _shipmentWithLineService.UpdateShipmentWithLineAsync(id, request, cancellationToken);
            return Ok(updatedShipmentWithLine);
        }

        /// <summary>
        /// 出荷記録テーブルから削除する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<DeleteWithUuidResponse>> DeleteShipmentWithLine(
            Guid id,
            CancellationToken cancellationToken)
        {
            var deletedResult = await _shipmentWithLineService.DeleteShipmentWithLineAsync(id, cancellationToken);
            return Ok(deletedResult);
        }

        /// <summary>
        /// 出荷記録テーブルを検索する
        /// </summary>
        /// <param name="request">検索用リクエスト（ページング・ソート含む）</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<SearchResponse<ShipmentWithLineListItemResponse>>> SearchShipmentWithLines(
            [FromQuery] ShipmentWithLineSearchRequest request,
            CancellationToken cancellationToken)
        {
            var searchResult = await _shipmentWithLineService.SearchShipmentWithLineAsync(request, cancellationToken);
            return Ok(searchResult);
        }
    }
}
