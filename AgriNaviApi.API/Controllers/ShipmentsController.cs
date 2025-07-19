using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Shipments;
using AgriNaviApi.Application.Responses.Shipments;
using AgriNaviApi.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShipmentsController : ControllerBase
    {
        private readonly IShipmentService _shipmentService;

        public ShipmentsController(IShipmentService shipmentService)
        {
            _shipmentService = shipmentService;
        }

        /// <summary>
        /// 出荷記録テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ShipmentCreateResponse>> CreateShipment(
            [FromBody] ShipmentCreateRequest request,
            CancellationToken cancellationToken)
        {
            var createdShipment = await _shipmentService.CreateShipmentAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetShipmentById), new { id = createdShipment.Uuid }, createdShipment);
        }

        /// <summary>
        /// 出荷記録テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ShipmentDetailResponse>> GetShipmentById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var shipment = await _shipmentService.GetShipmentByIdAsync(id, cancellationToken);
            return Ok(shipment);
        }

        /// <summary>
        /// 出荷記録テーブルを更新する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ShipmentUpdateResponse>> UpdateShipment(
            Guid id,
            [FromBody] ShipmentUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var updatedShipment = await _shipmentService.UpdateShipmentAsync(id, request, cancellationToken);
            return Ok(updatedShipment);
        }

        /// <summary>
        /// 出荷記録テーブルから削除する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<DeleteWithUuidResponse>> DeleteShipment(
            Guid id,
            CancellationToken cancellationToken)
        {
            var deletedResult = await _shipmentService.DeleteShipmentAsync(id, cancellationToken);
            return Ok(deletedResult);
        }

        /// <summary>
        /// 出荷記録テーブルを検索する
        /// </summary>
        /// <param name="request">検索用リクエスト（ページング・ソート含む）</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<SearchResponse<ShipmentListItemResponse>>> SearchShipments(
            [FromQuery] ShipmentSearchRequest request,
            CancellationToken cancellationToken)
        {
            var searchResult = await _shipmentService.SearchShipmentAsync(request, cancellationToken);
            return Ok(searchResult);
        }
    }
}
