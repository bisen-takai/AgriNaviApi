using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.SeasonSchedules;
using AgriNaviApi.Application.Responses.SeasonSchedules;
using AgriNaviApi.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AgriNaviApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SeasonSchedulesController : ControllerBase
    {
        private readonly ISeasonScheduleService _seasonScheduleService;

        public SeasonSchedulesController(ISeasonScheduleService seasonScheduleService)
        {
            _seasonScheduleService = seasonScheduleService;
        }

        /// <summary>
        /// 作付計画テーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<SeasonScheduleCreateResponse>> CreateSeasonSchedule(
            [FromBody] SeasonScheduleCreateRequest request,
            CancellationToken cancellationToken)
        {
            var createdSeasonSchedule = await _seasonScheduleService.CreateSeasonScheduleAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetSeasonScheduleById), new { id = createdSeasonSchedule.Uuid }, createdSeasonSchedule);
        }

        /// <summary>
        /// 作付計画テーブルの詳細情報を取得する
        /// </summary>
        /// <param name="id">作付計画ID</param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SeasonScheduleDetailResponse>> GetSeasonScheduleById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var seasonSchedule = await _seasonScheduleService.GetSeasonScheduleByIdAsync(id, cancellationToken);
            return Ok(seasonSchedule);
        }

        /// <summary>
        /// 作付計画テーブルを更新する
        /// </summary>
        /// <param name="id">作付計画ID</param>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<SeasonScheduleUpdateResponse>> UpdateSeasonSchedule(
            Guid id,
            [FromBody] SeasonScheduleUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var updatedSeasonSchedule = await _seasonScheduleService.UpdateSeasonScheduleAsync(id, request, cancellationToken);
            return Ok(updatedSeasonSchedule);
        }

        /// <summary>
        /// 作付計画テーブルから削除する
        /// </summary>
        /// <param name="id">作付計画ID</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<DeleteWithUuidResponse>> DeleteSeasonSchedule(
            Guid id,
            CancellationToken cancellationToken)
        {
            var deletedResult = await _seasonScheduleService.DeleteSeasonScheduleAsync(id, cancellationToken);
            return Ok(deletedResult);
        }

        /// <summary>
        /// 作付計画テーブルを検索する
        /// </summary>
        /// <param name="request">検索用リクエスト（ページング・ソート含む）</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<SearchResponse<SeasonScheduleListItemResponse>>> SearchSeasonSchedules(
            [FromQuery] SeasonScheduleSearchRequest request,
            CancellationToken cancellationToken)
        {
            var searchResult = await _seasonScheduleService.SearchSeasonScheduleAsync(request, cancellationToken);
            return Ok(searchResult);
        }
    }
}
