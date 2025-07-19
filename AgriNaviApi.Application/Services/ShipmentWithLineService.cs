using AgriNaviApi.Application.Common;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.ShipmentLines;
using AgriNaviApi.Application.Requests.ShipmentWithLines;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.ShipmentWithLines;
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
    /// 出荷記録テーブルに関するサービス処理
    /// </summary>
    public class ShipmentWithLineService : IShipmentWithLineService
    {
        const string TableName = "出荷記録";

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IForeignKeyValidator _fkValidator;

        public ShipmentWithLineService(AppDbContext context, IMapper mapper, IForeignKeyValidator fkValidator)
        {
            _context = context;
            _mapper = mapper;
            _fkValidator = fkValidator;
        }

        /// <summary>
        /// 出荷記録テーブルに登録する
        /// </summary>
        /// <param name="request">出荷記録登録用リクエストデータ</param>
        /// <returns>出荷記録登録情報</returns>
        /// <exception cref="DuplicateEntityException">出荷記録が登録済みの場合にスローされる</exception>
        public async Task<ShipmentWithLineCreateResponse> CreateShipmentWithLineAsync(
            ShipmentWithLineCreateRequest request,
            CancellationToken cancellationToken)
        {
            // null対策：foreachやSelect前に必ず補正
            request.Details ??= new List<ShipmentLineCreateRequest>();

            await _fkValidator.ValidateFieldExistsAsync(request.FieldId, cancellationToken);

            await _fkValidator.ValidateSeasonScheduleExistsAsync(request.SeasonScheduleId, cancellationToken);

            await _fkValidator.ValidateShipmentWithLineExistsAsync(request.Details, cancellationToken);

            // 出荷記録名はリクエストの値をマッピング
            var shipmentWithLine = _mapper.Map<ShipmentEntity>(request);

            _context.Shipments.Add(shipmentWithLine);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (DbExceptionHelper.IsUniqueConstraintViolation(ex))
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { TableName }
                );
            }

            return _mapper.Map<ShipmentWithLineCreateResponse>(shipmentWithLine);
        }

        /// <summary>
        /// 出荷記録テーブル詳細を取得する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <returns>出荷記録詳細情報</returns>
        /// <exception cref="EntityNotFoundException">出荷記録が見つからなかった場合にスローされる</exception>
        public async Task<ShipmentWithLineDetailResponse> GetShipmentWithLineByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var shipmentWithLine = await _context.Shipments
                .Where(s => !s.IsDeleted)
                .Include(s => s.Field)
                .Include(s => s.SeasonSchedule)
                .Include(s => s.Lines.Where(l => !l.IsDeleted))
                    .ThenInclude(l => l.ShipDestination)
                .Include(s => s.Lines.Where(l => !l.IsDeleted))
                    .ThenInclude(l => l.QualityStandard)
                .Include(s => s.Lines.Where(l => !l.IsDeleted))
                    .ThenInclude(l => l.Unit)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Uuid == id, cancellationToken);

            if (shipmentWithLine == null)
            {
                // エンティティが存在しない場合は、例外を投げる
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<ShipmentWithLineDetailResponse>(shipmentWithLine);
        }

        /// <summary>
        /// 出荷記録テーブルを更新する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <param name="request">出荷記録更新用リクエストデータ</param>
        /// <returns>出荷記録更新情報</returns>
        /// <exception cref="EntityNotFoundException">更新対象の出荷記録が見つからなかった場合にスローされる</exception>
        /// <exception cref="DuplicateEntityException">出荷記録名が重複している場合にスローされる</exception>
        public async Task<ShipmentWithLineUpdateResponse> UpdateShipmentWithLineAsync(
            Guid id,
            ShipmentWithLineUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var shipmentWithLine = await _context.Shipments
                .Include(s => s.Lines)
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(f => f.Uuid == id, cancellationToken);

            if (shipmentWithLine == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            await _fkValidator.ValidateFieldExistsAsync(request.FieldId, cancellationToken);

            await _fkValidator.ValidateSeasonScheduleExistsAsync(request.SeasonScheduleId, cancellationToken);

            await _fkValidator.ValidateShipmentWithLineExistsAsync(request.Details, cancellationToken);

            // 出荷記録名はリクエストの値をマッピング
            shipmentWithLine = _mapper.Map(request, shipmentWithLine);

            var requestLineUuids = request.Details.Select(d => d.Uuid).ToHashSet();

            // 削除
            foreach (var line in shipmentWithLine.Lines.Where(l => !requestLineUuids.Contains(l.Uuid)))
            {
                line.IsDeleted = true;
                line.DeletedAt = DateTime.UtcNow;
            }

            // 更新 or 追加
            foreach (var detail in request.Details)
            {
                var existing = shipmentWithLine.Lines.FirstOrDefault(l => l.Uuid == detail.Uuid);
                if (existing != null)
                {
                    _mapper.Map(detail, existing); // 更新
                }
                else
                {
                    var newLine = _mapper.Map<ShipmentLineEntity>(detail);
                    shipmentWithLine.Lines.Add(newLine); // 追加
                }
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (DbExceptionHelper.IsUniqueConstraintViolation(ex))
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { TableName }
                );
            }

            return _mapper.Map<ShipmentWithLineUpdateResponse>(shipmentWithLine);
        }

        /// <summary>
        /// 出荷記録テーブルから削除する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <returns>削除結果</returns>
        /// <exception cref="EntityNotFoundException">削除対象の出荷記録が見つからなかった場合にスローされる</exception>
        public async Task<DeleteWithUuidResponse> DeleteShipmentWithLineAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var shipmentWithLine = await _context.Shipments
                .Include(s => s.Lines)
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(f => f.Uuid == id, cancellationToken);

            if (shipmentWithLine == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            shipmentWithLine.IsDeleted = true;
            shipmentWithLine.DeletedAt = DateTime.UtcNow;

            foreach (var line in shipmentWithLine.Lines)
            {
                line.IsDeleted = true;
                line.DeletedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteWithUuidResponse
            {
                Uuid = shipmentWithLine.Uuid,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, TableName),
                DeletedAt = shipmentWithLine.DeletedAt
            };
        }

        /// <summary>
        /// 出荷記録テーブルを検索する
        /// </summary>
        /// <param name="request">出荷記録検索用リクエストデータ</param>
        /// <returns>検索結果</returns>
        public async Task<SearchResponse<ShipmentWithLineListItemResponse>> SearchShipmentWithLineAsync(
            ShipmentWithLineSearchRequest request,
            CancellationToken cancellationToken)
        {
            // 同一トランザクション内で複数回 SELECT を実行しても、最初の時点のスナップショットを参照し続ける
            using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead, cancellationToken);

            // shipmentWithLinesテーブルからクエリ可能なIQueryableを取得
            var query = _context.Shipments
                .Include(s => s.Field)
                .Include(s => s.SeasonSchedule)
                .Where(s => !s.IsDeleted)
                .Include(s => s.Lines.Where(l => !l.IsDeleted))
                    .ThenInclude(l => l.ShipDestination)
                .Include(s => s.Lines.Where(l => !l.IsDeleted))
                    .ThenInclude(l => l.QualityStandard)
                .Include(s => s.Lines.Where(l => !l.IsDeleted))
                    .ThenInclude(l => l.Unit)
                .AsNoTracking()
                .AsQueryable();

            if (request.SeasonScheduleId > 0)
            {
                query = query.Where(r => r.SeasonScheduleId == request.SeasonScheduleId);
            }
            else
            {
                if (request.StartDate != null)
                {
                    // request.StartDate 以降のデータを取得
                    query = query.Where(c => c.ShipmentDate >= request.StartDate);
                }

                if (request.EndDate != null)
                {
                    // request.EndDate 以下のデータを取得
                    query = query.Where(c => c.ShipmentDate <= request.EndDate);
                }
            }

            switch (request.SortBy)
            {
                case ShipmentSortKey.Date:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.ShipmentDate);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.ShipmentDate);
                    }
                    break;
                case ShipmentSortKey.Field:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.Field.Name);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.Field.Name);
                    }
                    break;
                case ShipmentSortKey.SeasonSchedule:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.SeasonSchedule.Name);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.SeasonSchedule.Name);
                    }
                    break;
                default:
                    query = query.OrderBy(c => c.ShipmentDate);
                    break;
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            // トランザクションは読み取り専用なのでコミットだけ
            await tx.CommitAsync(cancellationToken);

            return new SearchResponse<ShipmentWithLineListItemResponse>
            {
                SearchItems = _mapper.Map<List<ShipmentWithLineListItemResponse>>(items),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
