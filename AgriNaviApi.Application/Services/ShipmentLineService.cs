using AgriNaviApi.Application.Common;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.ShipmentLines;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.ShipmentLines;
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
    /// 出荷記録詳細テーブルに関するサービス処理
    /// </summary>
    public class ShipmentLineService : IShipmentLineService
    {
        const string TableName = "出荷記録詳細";

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IForeignKeyValidator _fkValidator;

        public ShipmentLineService(AppDbContext context, IMapper mapper, IForeignKeyValidator fkValidator)
        {
            _context = context;
            _mapper = mapper;
            _fkValidator = fkValidator;
        }

        /// <summary>
        /// 出荷記録詳細テーブルに登録する
        /// </summary>
        /// <param name="request">出荷記録詳細登録用リクエストデータ</param>
        /// <returns>出荷記録詳細登録情報</returns>
        /// <exception cref="DuplicateEntityException">出荷記録詳細が登録済みの場合にスローされる</exception>
        public async Task<ShipmentLineCreateResponse> CreateShipmentLineAsync(
            ShipmentLineCreateRequest request,
            CancellationToken cancellationToken)
        {
            await _fkValidator.ValidateShipmentExistsAsync(request.ShipmentId, cancellationToken);

            await _fkValidator.ValidateShipDestinationExistsAsync(request.ShipDestinationId, cancellationToken);

            await _fkValidator.ValidateQualityStandardExistsAsync(request.QualityStandardId, cancellationToken);

            await _fkValidator.ValidateUnitExistsAsync(request.UnitId, cancellationToken);

            // 出荷記録詳細名はリクエストの値をマッピング
            var shipmentLine = _mapper.Map<ShipmentLineEntity>(request);

            _context.ShipmentLines.Add(shipmentLine);

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

            return _mapper.Map<ShipmentLineCreateResponse>(shipmentLine);
        }

        /// <summary>
        /// 出荷記録詳細テーブル詳細を取得する
        /// </summary>
        /// <param name="id">出荷記録詳細ID</param>
        /// <returns>出荷記録詳細詳細情報</returns>
        /// <exception cref="EntityNotFoundException">出荷記録詳細が見つからなかった場合にスローされる</exception>
        public async Task<ShipmentLineDetailResponse> GetShipmentLineByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var shipmentLine = await _context.ShipmentLines
                .Include(f => f.Shipment)
                .Include(f => f.ShipDestination)
                .Include(f => f.QualityStandard)
                .Include(f => f.Unit)
                .Where(s => !s.IsDeleted)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Uuid == id, cancellationToken);

            if (shipmentLine == null)
            {
                // エンティティが存在しない場合は、例外を投げる
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<ShipmentLineDetailResponse>(shipmentLine);
        }

        /// <summary>
        /// 出荷記録詳細テーブルを更新する
        /// </summary>
        /// <param name="id">出荷記録詳細ID</param>
        /// <param name="request">出荷記録詳細更新用リクエストデータ</param>
        /// <returns>出荷記録詳細更新情報</returns>
        /// <exception cref="EntityNotFoundException">更新対象の出荷記録詳細が見つからなかった場合にスローされる</exception>
        /// <exception cref="DuplicateEntityException">出荷記録詳細名が重複している場合にスローされる</exception>
        public async Task<ShipmentLineUpdateResponse> UpdateShipmentLineAsync(
            Guid id,
            ShipmentLineUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var shipmentLine = await _context.ShipmentLines
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(f => f.Uuid == id, cancellationToken);

            if (shipmentLine == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            await _fkValidator.ValidateShipmentExistsAsync(request.ShipmentId, cancellationToken);

            await _fkValidator.ValidateShipDestinationExistsAsync(request.ShipDestinationId, cancellationToken);

            await _fkValidator.ValidateQualityStandardExistsAsync(request.QualityStandardId, cancellationToken);

            await _fkValidator.ValidateUnitExistsAsync(request.UnitId, cancellationToken);

            // 出荷記録詳細名はリクエストの値をマッピング
            shipmentLine = _mapper.Map(request, shipmentLine);

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

            return _mapper.Map<ShipmentLineUpdateResponse>(shipmentLine);
        }

        /// <summary>
        /// 出荷記録詳細テーブルから削除する
        /// </summary>
        /// <param name="id">出荷記録詳細ID</param>
        /// <returns>削除結果</returns>
        /// <exception cref="EntityNotFoundException">削除対象の出荷記録詳細が見つからなかった場合にスローされる</exception>
        public async Task<DeleteWithUuidResponse> DeleteShipmentLineAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var shipmentLine = await _context.ShipmentLines
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(f => f.Uuid == id, cancellationToken);

            if (shipmentLine == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            shipmentLine.IsDeleted = true;
            shipmentLine.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteWithUuidResponse
            {
                Uuid = shipmentLine.Uuid,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, TableName),
                DeletedAt = shipmentLine.DeletedAt
            };
        }

        /// <summary>
        /// 出荷記録詳細テーブルを検索する
        /// </summary>
        /// <param name="request">出荷記録詳細検索用リクエストデータ</param>
        /// <returns>検索結果</returns>
        public async Task<SearchResponse<ShipmentLineListItemResponse>> SearchShipmentLineAsync(
            ShipmentLineSearchRequest request,
            CancellationToken cancellationToken)
        {
            // 同一トランザクション内で複数回 SELECT を実行しても、最初の時点のスナップショットを参照し続ける
            using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead, cancellationToken);

            // shipmentLinesテーブルからクエリ可能なIQueryableを取得
            var query = _context.ShipmentLines
                .Include(f => f.Shipment)
                .Include(f => f.ShipDestination)
                .Include(f => f.QualityStandard)
                .Include(f => f.Unit)
                .Where(s => !s.IsDeleted)
                .AsNoTracking()
                .AsQueryable();

            switch (request.SortBy)
            {
                case ShipmentLineSortKey.Shipment:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.ShipmentId);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.ShipmentId);
                    }
                    break;
                case ShipmentLineSortKey.ShipDestinationName:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.ShipDestination.Name);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.ShipDestination.Name);
                    }
                    break;
                case ShipmentLineSortKey.QualityStandardName:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.QualityStandard.Name);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.QualityStandard.Name);
                    }
                    break;
                default:
                    query = query.OrderBy(c => c.ShipmentId);
                    break;
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            // トランザクションは読み取り専用なのでコミットだけ
            await tx.CommitAsync(cancellationToken);

            return new SearchResponse<ShipmentLineListItemResponse>
            {
                SearchItems = _mapper.Map<List<ShipmentLineListItemResponse>>(items),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
