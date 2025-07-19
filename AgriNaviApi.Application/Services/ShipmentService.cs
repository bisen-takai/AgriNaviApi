using AgriNaviApi.Application.Common;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Shipments;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.Shipments;
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
    public class ShipmentService : IShipmentService
    {
        const string TableName = "出荷記録";

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IForeignKeyValidator _fkValidator;

        public ShipmentService(AppDbContext context, IMapper mapper, IForeignKeyValidator fkValidator)
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
        public async Task<ShipmentCreateResponse> CreateShipmentAsync(
            ShipmentCreateRequest request,
            CancellationToken cancellationToken)
        {
            await _fkValidator.ValidateFieldExistsAsync(request.FieldId, cancellationToken);

            await _fkValidator.ValidateSeasonScheduleExistsAsync(request.SeasonScheduleId, cancellationToken);

            // 出荷記録名はリクエストの値をマッピング
            var shipment = _mapper.Map<ShipmentEntity>(request);

            _context.Shipments.Add(shipment);

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

            return _mapper.Map<ShipmentCreateResponse>(shipment);
        }

        /// <summary>
        /// 出荷記録テーブル詳細を取得する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <returns>出荷記録詳細情報</returns>
        /// <exception cref="EntityNotFoundException">出荷記録が見つからなかった場合にスローされる</exception>
        public async Task<ShipmentDetailResponse> GetShipmentByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var shipment = await _context.Shipments
                .Include(f => f.Field)
                .Include(f => f.SeasonSchedule)
                .Where(s => !s.IsDeleted)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Uuid == id, cancellationToken);

            if (shipment == null)
            {
                // エンティティが存在しない場合は、例外を投げる
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<ShipmentDetailResponse>(shipment);
        }

        /// <summary>
        /// 出荷記録テーブルを更新する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <param name="request">出荷記録更新用リクエストデータ</param>
        /// <returns>出荷記録更新情報</returns>
        /// <exception cref="EntityNotFoundException">更新対象の出荷記録が見つからなかった場合にスローされる</exception>
        /// <exception cref="DuplicateEntityException">出荷記録名が重複している場合にスローされる</exception>
        public async Task<ShipmentUpdateResponse> UpdateShipmentAsync(
            Guid id,
            ShipmentUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var shipment = await _context.Shipments
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(f => f.Uuid == id, cancellationToken);

            if (shipment == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            await _fkValidator.ValidateFieldExistsAsync(request.FieldId, cancellationToken);

            await _fkValidator.ValidateSeasonScheduleExistsAsync(request.SeasonScheduleId, cancellationToken);

            // 出荷記録名はリクエストの値をマッピング
            shipment = _mapper.Map(request, shipment);

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

            return _mapper.Map<ShipmentUpdateResponse>(shipment);
        }

        /// <summary>
        /// 出荷記録テーブルから削除する
        /// </summary>
        /// <param name="id">出荷記録ID</param>
        /// <returns>削除結果</returns>
        /// <exception cref="EntityNotFoundException">削除対象の出荷記録が見つからなかった場合にスローされる</exception>
        public async Task<DeleteWithUuidResponse> DeleteShipmentAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var shipment = await _context.Shipments
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(f => f.Uuid == id, cancellationToken);

            if (shipment == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            shipment.IsDeleted = true;
            shipment.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteWithUuidResponse
            {
                Uuid = shipment.Uuid,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, TableName),
                DeletedAt = shipment.DeletedAt
            };
        }

        /// <summary>
        /// 出荷記録テーブルを検索する
        /// </summary>
        /// <param name="request">出荷記録検索用リクエストデータ</param>
        /// <returns>検索結果</returns>
        public async Task<SearchResponse<ShipmentListItemResponse>> SearchShipmentAsync(
            ShipmentSearchRequest request,
            CancellationToken cancellationToken)
        {
            // 同一トランザクション内で複数回 SELECT を実行しても、最初の時点のスナップショットを参照し続ける
            using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead, cancellationToken);

            // shipmentsテーブルからクエリ可能なIQueryableを取得
            var query = _context.Shipments
                .Include(f => f.Field)
                .Include(f => f.SeasonSchedule)
                .Where(s => !s.IsDeleted)
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

            return new SearchResponse<ShipmentListItemResponse>
            {
                SearchItems = _mapper.Map<List<ShipmentListItemResponse>>(items),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
