using AgriNaviApi.Application.DTOs.SeasonCropSchedules;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.SeasonCropSchedules;
using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class SeasonCropScheduleController : Controller
    {
        private readonly ISeasonCropScheduleService _seasonCropScheduleService;

        public SeasonCropScheduleController(ISeasonCropScheduleService seasonCropScheduleService)
        {
            _seasonCropScheduleService = seasonCropScheduleService;
        }

        /// <summary>
        /// 作付計画テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<SeasonCropScheduleCreateDto>> CreateSeasonCropSchedule([FromBody] SeasonCropScheduleCreateRequest request)
        {
            try
            {
                var createdSeasonCropSchedule = await _seasonCropScheduleService.CreateSeasonCropScheduleAsync(request);
                return CreatedAtAction(nameof(GetSeasonCropScheduleById), new { id = createdSeasonCropSchedule.Id }, createdSeasonCropSchedule);
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
        /// 作付計画テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SeasonCropScheduleDetailDto>> GetSeasonCropScheduleById(int id)
        {
            try
            {
                var createdSeasonCropSchedule = await _seasonCropScheduleService.GetSeasonCropScheduleByIdAsync(id);
                return Ok(createdSeasonCropSchedule);

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
        /// 作付計画テーブルを更新する
        /// </summary>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<SeasonCropScheduleUpdateDto>> UpdateSeasonCropSchedule(int id, [FromBody] SeasonCropScheduleUpdateRequest request)
        {
            try
            {
                var updatedSeasonCropSchedule = await _seasonCropScheduleService.UpdateSeasonCropScheduleAsync(request);
                return Ok(updatedSeasonCropSchedule);
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
        /// 作付計画テーブルから削除する
        /// </summary>
        /// <param name="request">削除用リクエストデータ</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeasonCropSchedule(int id)
        {
            try
            {
                var deletedResult = await _seasonCropScheduleService.DeleteSeasonCropScheduleAsync(id);
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
        /// 作付計画テーブルを検索する
        /// </summary>
        /// <param name="searchSeasonCropScheduleName">作付計画名</param>
        /// <param name="searchMatchType">検索の種類</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SearchSeasonCropSchedules(
            [FromQuery] string? searchSeasonCropScheduleName,
            [FromQuery] int? cropId,
            [FromQuery] SearchMatchType searchMatchType = SearchMatchType.None,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                var request = new SeasonCropScheduleSearchRequest
                {
                    SearchSeasonCropScheduleName = searchSeasonCropScheduleName,
                    CropId = cropId,
                    SearchMatchType = searchMatchType,
                    StartDate = startDate,
                    EndDate = endDate
                };

                var searchResult = await _seasonCropScheduleService.SearchSeasonCropScheduleAsync(request);
                return Ok(searchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
