using AgriNaviApi.Application.Common;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.SeasonSchedules;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.SeasonSchedules;
using AgriNaviApi.Infrastructure.Persistence.Contexts;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AgriNaviApi.Shared.Enums;
using AgriNaviApi.Shared.Exceptions;
using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AgriNaviApi.Application.Services
{
    /// <summary>
    /// 作付計画テーブルに関するサービス処理
    /// </summary>
    public class SeasonScheduleService : ISeasonScheduleService
    {
        const string TableName = "作付計画";

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IForeignKeyValidator _fkValidator;

        public SeasonScheduleService(AppDbContext context, IMapper mapper, IForeignKeyValidator fkValidator)
        {
            _context = context;
            _mapper = mapper;
            _fkValidator = fkValidator;
        }

        /// <summary>
        /// 作付計画テーブルに登録する
        /// </summary>
        /// <param name="request">作付計画登録用リクエストデータ</param>
        /// <returns>作付計画登録情報</returns>
        /// <exception cref="DuplicateEntityException">作付計画が登録済みの場合にスローされる</exception>
        public async Task<SeasonScheduleCreateResponse> CreateSeasonScheduleAsync(
            SeasonScheduleCreateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();

            // 入力値をトリミングしてDBの値と比較
            var exists = await _context.SeasonSchedules
                .AsNoTracking()
                .AnyAsync(c => c.Name == trimmedInput, cancellationToken);

            // 作付計画が既に登録されているかをチェック
            if (exists)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            await _fkValidator.ValidateCropExistsAsync(request.CropId, cancellationToken);

            // 余計な前後スペースを取り除いたNameでリクエストデータを更新する
            var normalizedRequest = new SeasonScheduleCreateRequest
            {
                Name = trimmedInput,
                CropId = request.CropId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Remark = request.Remark
            };

            // 作付計画名はリクエストの値をマッピング
            var seasonSchedule = _mapper.Map<SeasonScheduleEntity>(normalizedRequest);

            _context.SeasonSchedules.Add(seasonSchedule);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (DbExceptionHelper.IsUniqueConstraintViolation(ex))
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            return _mapper.Map<SeasonScheduleCreateResponse>(seasonSchedule);
        }

        /// <summary>
        /// 作付計画テーブル詳細を取得する
        /// </summary>
        /// <param name="id">作付計画ID</param>
        /// <returns>作付計画詳細情報</returns>
        /// <exception cref="EntityNotFoundException">作付計画が見つからなかった場合にスローされる</exception>
        public async Task<SeasonScheduleDetailResponse> GetSeasonScheduleByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var seasonSchedule = await _context.SeasonSchedules
                .Include(f => f.Crop)
                .Where(s => !s.IsDeleted)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Uuid == id, cancellationToken);

            if (seasonSchedule == null)
            {
                // エンティティが存在しない場合は、例外を投げる
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<SeasonScheduleDetailResponse>(seasonSchedule);
        }

        /// <summary>
        /// 作付計画テーブルを更新する
        /// </summary>
        /// <param name="id">作付計画ID</param>
        /// <param name="request">作付計画更新用リクエストデータ</param>
        /// <returns>作付計画更新情報</returns>
        /// <exception cref="EntityNotFoundException">更新対象の作付計画が見つからなかった場合にスローされる</exception>
        /// <exception cref="DuplicateEntityException">作付計画名が重複している場合にスローされる</exception>
        public async Task<SeasonScheduleUpdateResponse> UpdateSeasonScheduleAsync(
            Guid id,
            SeasonScheduleUpdateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();

            // 入力値をトリミングしてDBの値と比較し、更新対象を検索
            var existingSeasonSchedule = await _context.SeasonSchedules
                .AsNoTracking()
                .Where(c => c.Name == trimmedInput && !c.IsDeleted)
                .SingleOrDefaultAsync(cancellationToken);

            if (existingSeasonSchedule != null && existingSeasonSchedule.Uuid != id)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            await _fkValidator.ValidateCropExistsAsync(request.CropId, cancellationToken);

            var seasonSchedule = await _context.SeasonSchedules
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(f => f.Uuid == id, cancellationToken);

            if (seasonSchedule == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // 作付計画名はリクエストの値をマッピング
            seasonSchedule = _mapper.Map(request, seasonSchedule);

            // マッピング後に Name はトリム済みの trimmedInput で上書きする
            seasonSchedule.Name = trimmedInput;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (DbExceptionHelper.IsUniqueConstraintViolation(ex))
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            return _mapper.Map<SeasonScheduleUpdateResponse>(seasonSchedule);
        }

        /// <summary>
        /// 作付計画テーブルから削除する
        /// </summary>
        /// <param name="id">作付計画ID</param>
        /// <returns>削除結果</returns>
        /// <exception cref="EntityNotFoundException">削除対象の作付計画が見つからなかった場合にスローされる</exception>
        public async Task<DeleteWithUuidResponse> DeleteSeasonScheduleAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var seasonSchedule = await _context.SeasonSchedules
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(f => f.Uuid == id, cancellationToken);

            if (seasonSchedule == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            seasonSchedule.IsDeleted = true;
            seasonSchedule.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteWithUuidResponse
            {
                Uuid = seasonSchedule.Uuid,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, seasonSchedule.Name),
                DeletedAt = seasonSchedule.DeletedAt
            };
        }

        /// <summary>
        /// 作付計画テーブルを検索する
        /// </summary>
        /// <param name="request">作付計画検索用リクエストデータ</param>
        /// <returns>検索結果</returns>
        public async Task<SearchResponse<SeasonScheduleListItemResponse>> SearchSeasonScheduleAsync(
            SeasonScheduleSearchRequest request,
            CancellationToken cancellationToken)
        {
            // 同一トランザクション内で複数回 SELECT を実行しても、最初の時点のスナップショットを参照し続ける
            using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead, cancellationToken);

            // seasonSchedulesテーブルからクエリ可能なIQueryableを取得
            var query = _context.SeasonSchedules
                .Include(f => f.Crop)
                .Where(s => !s.IsDeleted)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchName))
            {
                // 余計な前後スペースだけ取り除く
                var trimmedSearch = request.SearchName.Trim();

                switch (request.SearchMatchType)
                {
                    case SearchMatchType.Exact:
                        query = query.Where(c => c.Name == trimmedSearch);
                        break;
                    case SearchMatchType.Prefix:
                        query = query.Where(c => c.Name.StartsWith(trimmedSearch));
                        break;
                    case SearchMatchType.Suffix:
                        query = query.Where(c => c.Name.EndsWith(trimmedSearch));
                        break;
                    case SearchMatchType.Partial:
                        query = query.Where(c => c.Name.Contains(trimmedSearch));
                        break;
                    case SearchMatchType.None:
                    default:
                        // 全件検索とする
                        break;
                }
            }

            switch (request.SortBy)
            {
                case SeasonScheduleSortKey.Name:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.Name);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.Name);
                    }
                    break;
                case SeasonScheduleSortKey.Crop:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.Crop.Name);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.Crop.Name);
                    }
                    break;
                case SeasonScheduleSortKey.StartDate:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.StartDate);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.StartDate);
                    }
                    break;
                case SeasonScheduleSortKey.EndDate:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.EndDate);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.EndDate);
                    }
                    break;
                default:
                    query = query.OrderBy(c => c.Id);
                    break;
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            // トランザクションは読み取り専用なのでコミットだけ
            await tx.CommitAsync(cancellationToken);

            return new SearchResponse<SeasonScheduleListItemResponse>
            {
                SearchItems = _mapper.Map<List<SeasonScheduleListItemResponse>>(items),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
