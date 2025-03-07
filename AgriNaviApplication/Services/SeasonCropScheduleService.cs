using AgriNaviApi.Application.DTOs.SeasonCropSchedules;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.SeasonCropSchedules;
using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Exceptions;
using AgriNaviApi.Common.Interfaces;
using AgriNaviApi.Common.Resources;
using AgriNaviApi.Infrastructure.Persistence.Contexts;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AgriNaviApi.Application.Services
{
    /// <summary>
    /// 作付計画テーブルに関するサービス処理
    /// </summary>
    public class SeasonCropScheduleService : ISeasonCropScheduleService
    {
        const string TableName = "作付計画";

        private readonly AppDbContext _context;
        private readonly IUuidGenerator _uuidGenerator;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public SeasonCropScheduleService(AppDbContext context, IUuidGenerator uuidGenerator, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _uuidGenerator = uuidGenerator;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// 作付計画テーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DuplicateEntityException"></exception>
        public async Task<SeasonCropScheduleCreateDto> CreateSeasonCropScheduleAsync(SeasonCropScheduleCreateRequest request)
        {
            // 作付計画名が既に登録されているかをチェック
            var exists = await _context.SeasonCropSchedules.AnyAsync(u => u.Name == request.Name && !u.IsDeleted);
            if (exists)
            {
                string message = string.Format(CommonErrorMessages.DuplicateErrorMessage, request.Name);
                throw new DuplicateEntityException(message);
            }

            // 計画開始年月と計画終了年月のKindプロパティが異なるかをチェック
            if (request.StartDate.Kind != request.EndDate.Kind)
            {
                string message = string.Format(CommonErrorMessages.DuplicateErrorMessage, request.Name);
                throw new DuplicateEntityException(message);
            }

            // 計画終了年月が計画開始年月以降であるかをチェック
            if (request.StartDate > request.EndDate)
            {
                string message = string.Format(CommonErrorMessages.DuplicateErrorMessage, request.Name);
                throw new DuplicateEntityException(message);
            }

            // 作付計画名はリクエストの値をマッピング
            var seasonCropSchedule = _mapper.Map<SeasonCropScheduleEntity>(request);

            seasonCropSchedule.Uuid = _uuidGenerator.GenerateUuid();
            seasonCropSchedule.CreatedAt = _dateTimeProvider.UtcNow;
            seasonCropSchedule.LastUpdatedAt = _dateTimeProvider.UtcNow;

            _context.SeasonCropSchedules.Add(seasonCropSchedule);
            await _context.SaveChangesAsync();

            return _mapper.Map<SeasonCropScheduleCreateDto>(seasonCropSchedule);
        }

        /// <summary>
        /// 作付計画テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<SeasonCropScheduleDetailDto> GetSeasonCropScheduleByIdAsync(int id)
        {
            var seasonCropSchedule = await _context.SeasonCropSchedules.FindAsync(id);

            if (seasonCropSchedule == null || seasonCropSchedule.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                // エンティティが存在しない場合は、例外を投げる
                throw new KeyNotFoundException(message);
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<SeasonCropScheduleDetailDto>(seasonCropSchedule);
        }

        /// <summary>
        /// 作付計画テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<SeasonCropScheduleUpdateDto> UpdateSeasonCropScheduleAsync(SeasonCropScheduleUpdateRequest request)
        {
            var seasonCropSchedule = await _context.SeasonCropSchedules.FindAsync(request.Id);

            if (seasonCropSchedule == null || seasonCropSchedule.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new KeyNotFoundException(message);
            }

            // 計画開始年月と計画終了年月のKindプロパティが異なるかをチェック
            if (request.StartDate.Kind != request.EndDate.Kind)
            {
                string message = string.Format(CommonErrorMessages.DuplicateErrorMessage, request.Name);
                throw new DuplicateEntityException(message);
            }

            // 計画終了年月が計画開始年月以降であるかをチェック
            if (request.StartDate > request.EndDate)
            {
                string message = string.Format(CommonErrorMessages.DuplicateErrorMessage, request.Name);
                throw new DuplicateEntityException(message);
            }

            // 作付計画名はリクエストの値をマッピング
            seasonCropSchedule = _mapper.Map(request, seasonCropSchedule);

            seasonCropSchedule.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<SeasonCropScheduleUpdateDto>(seasonCropSchedule);
        }

        /// <summary>
        /// 作付計画テーブルから削除する※削除フラグをtrueにするのみで、実際は削除しない
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<SeasonCropScheduleDeleteDto> DeleteSeasonCropScheduleAsync(SeasonCropScheduleDeleteRequest request)
        {
            var seasonCropSchedule = await _context.SeasonCropSchedules.FindAsync(request.Id);

            if (seasonCropSchedule == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new InvalidOperationException(message);
            }

            if (seasonCropSchedule.IsDeleted)
            {
                // 既に削除済みの場合
                string message = string.Format(CommonErrorMessages.DeletedMessage, TableName);
                throw new InvalidOperationException(message);
            }

            seasonCropSchedule.IsDeleted = true;
            seasonCropSchedule.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return new SeasonCropScheduleDeleteDto
            {
                Id = seasonCropSchedule.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, seasonCropSchedule.Name)
            };
        }

        /// <summary>
        /// 作付計画テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SeasonCropScheduleSearchDto> SearchSeasonCropScheduleAsync(SeasonCropScheduleSearchRequest request)
        {
            // seasonCropSchedulesテーブルからクエリ可能なIQueryableを取得
            var query = _context.SeasonCropSchedules.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchSeasonCropScheduleName))
            {
                switch (request.SearchMatchType)
                {
                    case SearchMatchType.EXACT:
                        query = query.Where(c => c.Name == request.SearchSeasonCropScheduleName && !c.IsDeleted);
                        break;
                    case SearchMatchType.PREFIX:
                        query = query.Where(c => c.Name.StartsWith(request.SearchSeasonCropScheduleName) && !c.IsDeleted);
                        break;
                    case SearchMatchType.SUFFIX:
                        query = query.Where(c => c.Name.EndsWith(request.SearchSeasonCropScheduleName) && !c.IsDeleted);
                        break;
                    case SearchMatchType.PARTIAL:
                        query = query.Where(c => c.Name.Contains(request.SearchSeasonCropScheduleName) && !c.IsDeleted);
                        break;
                    case SearchMatchType.None:
                    default:
                        // 全件検索とする
                        query = query.Where(c => !c.IsDeleted);
                        break;
                }
            }
            else
            {
                // 削除していないデータが対象
                query = query.Where(c => !c.IsDeleted);

                // EndDateが指定されているかどうかで条件を分ける
                if (request.StartDate != null && request.EndDate != null)
                {
                    // request.StartDate 以降かつ request.EndDate 以下のデータを取得
                    query = query.Where(c => c.StartDate >= request.StartDate && c.StartDate <= request.EndDate);
                }
                else if (request.StartDate != null)
                {
                    // request.StartDate 以降のデータを取得
                    query = query.Where(c => c.StartDate >= request.StartDate);
                }
                else if (request.EndDate != null)
                {
                    // request.EndDate 以下のデータを取得
                    query = query.Where(c => c.StartDate <= request.EndDate);
                }
            }

            var seasonCropSchedules = await query.ToListAsync();

            return new SeasonCropScheduleSearchDto
            {
                SearchItems = _mapper.Map<List<SeasonCropScheduleListItemDto>>(seasonCropSchedules),
                TotalCount = seasonCropSchedules.Count
            };
        }
    }
}
