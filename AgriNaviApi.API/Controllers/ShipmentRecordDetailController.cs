using AgriNaviApi.Application.DTOs.ShipmentRecordDetails;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.ShipmentRecordDetails;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ShipmentRecordDetailController : Controller
    {
        private readonly IShipmentRecordDetailService _shipmentRecordDetailService;

        public ShipmentRecordDetailController(IShipmentRecordDetailService shipmentRecordDetailService)
        {
            _shipmentRecordDetailService = shipmentRecordDetailService;
        }

        /// <summary>
        /// 出荷記録詳細テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ShipmentRecordDetailCreateDto>> CreateShipmentRecordDetail([FromBody] ShipmentRecordDetailCreateRequest request)
        {
            try
            {
                var createdShipmentRecordDetail = await _shipmentRecordDetailService.CreateShipmentRecordDetailAsync(request);
                return CreatedAtAction(nameof(GetShipmentRecordDetailById), new { id = createdShipmentRecordDetail.Id }, createdShipmentRecordDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// 出荷記録詳細テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ShipmentRecordDetailDetailDto>> GetShipmentRecordDetailById(int id)
        {
            try
            {
                var createdShipmentRecordDetail = await _shipmentRecordDetailService.GetShipmentRecordDetailByIdAsync(id);
                return Ok(createdShipmentRecordDetail);

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
        /// 出荷記録詳細テーブルを更新する
        /// </summary>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ShipmentRecordDetailUpdateDto>> UpdateShipmentRecordDetail([FromBody] ShipmentRecordDetailUpdateRequest request)
        {
            try
            {
                var updatedShipmentRecordDetail = await _shipmentRecordDetailService.UpdateShipmentRecordDetailAsync(request);
                return Ok(updatedShipmentRecordDetail);
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
        /// 出荷記録詳細テーブルから削除する
        /// </summary>
        /// <param name="request">削除用リクエストデータ</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteShipmentRecordDetail([FromBody] ShipmentRecordDetailDeleteRequest request)
        {
            try
            {
                var deletedResult = await _shipmentRecordDetailService.DeleteShipmentRecordDetailAsync(request);
                return Ok(deletedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// 出荷記録詳細テーブルを検索する
        /// </summary>
        /// <param name="shipmentRecordId">出荷記録ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchShipmentRecordDetails(
            [FromQuery] int shipmentRecordId = 0)
        {
            try
            {
                var request = new ShipmentRecordDetailSearchRequest
                {
                    ShipmentRecordId = shipmentRecordId
                };

                var searchResult = await _shipmentRecordDetailService.SearchShipmentRecordDetailAsync(request);
                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
