using AgriNaviApi.Application.DTOs.ShipmentRecords;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.ShipmentRecords;
using AgriNaviApi.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ShipmentRecordController : Controller
    {
        private readonly IShipmentRecordService _shipmentRecordService;

        public ShipmentRecordController(IShipmentRecordService shipmentRecordService)
        {
            _shipmentRecordService = shipmentRecordService;
        }

        /// <summary>
        /// 出荷記録テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ShipmentRecordCreateDto>> CreateShipmentRecord([FromBody] ShipmentRecordCreateRequest request)
        {
            try
            {
                var createdShipmentRecord = await _shipmentRecordService.CreateShipmentRecordAsync(request);
                return CreatedAtAction(nameof(GetShipmentRecordById), new { id = createdShipmentRecord.Id }, createdShipmentRecord);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// 出荷記録テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ShipmentRecordDetailDto>> GetShipmentRecordById(int id)
        {
            try
            {
                var createdShipmentRecord = await _shipmentRecordService.GetShipmentRecordByIdAsync(id);
                return Ok(createdShipmentRecord);

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
        /// 出荷記録テーブルを更新する
        /// </summary>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ShipmentRecordUpdateDto>> UpdateShipmentRecord([FromBody] ShipmentRecordUpdateRequest request)
        {
            try
            {
                var updatedShipmentRecord = await _shipmentRecordService.UpdateShipmentRecordAsync(request);
                return Ok(updatedShipmentRecord);
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
        /// 出荷記録テーブルから削除する
        /// </summary>
        /// <param name="request">削除用リクエストデータ</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteShipmentRecord([FromBody] ShipmentRecordDeleteRequest request)
        {
            try
            {
                var deletedResult = await _shipmentRecordService.DeleteShipmentRecordAsync(request);
                return Ok(deletedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// 出荷記録テーブルを検索する
        /// </summary>
        /// <param name="startDate">検索開始日</param>
        /// <param name="endDate">検索終了日</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchShipmentRecords(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var request = new ShipmentRecordSearchRequest
                {
                    StartDate = startDate,
                    EndDate = endDate
                };

                var searchResult = await _shipmentRecordService.SearchShipmentRecordAsync(request);
                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
