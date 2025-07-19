using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.ShipDestinations;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.ShipDestinations;
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
    /// 出荷先テーブルに関するサービス処理
    /// </summary>
    public class ShipDestinationService : IShipDestinationService
    {
        const string TableName = "出荷先";

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ShipDestinationService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// 出荷先テーブルに登録する
        /// </summary>
        /// <param name="request">出荷先登録用リクエストデータ</param>
        /// <returns>出荷先登録情報</returns>
        /// <exception cref="DuplicateEntityException">出荷先が登録済みの場合にスローされる</exception>
        public async Task<ShipDestinationCreateResponse> CreateShipDestinationAsync(
            ShipDestinationCreateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();

            // 入力値をトリミングしてDBの値と比較
            var exists = await _context.ShipDestinations
                .AsNoTracking()
                .AnyAsync(c => c.Name == trimmedInput, cancellationToken);

            // 出荷先が既に登録されているかをチェック
            if (exists)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            // 出荷先名はリクエストの値をマッピング
            var shipDestination = _mapper.Map<ShipDestinationEntity>(request);

            _context.ShipDestinations.Add(shipDestination);

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

            return _mapper.Map<ShipDestinationCreateResponse>(shipDestination);
        }

        /// <summary>
        /// 出荷先テーブル詳細を取得する
        /// </summary>
        /// <param name="id">出荷先ID</param>
        /// <returns>出荷先詳細情報</returns>
        /// <exception cref="EntityNotFoundException">出荷先が見つからなかった場合にスローされる</exception>
        public async Task<ShipDestinationDetailResponse> GetShipDestinationByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var shipDestination = await _context.ShipDestinations.FindAsync(id, cancellationToken);

            if (shipDestination == null)
            {
                // エンティティが存在しない場合は、例外を投げる
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<ShipDestinationDetailResponse>(shipDestination);
        }

        /// <summary>
        /// 出荷先テーブルを更新する
        /// </summary>
        /// <param name="id">出荷先ID</param>
        /// <param name="request">出荷先更新用リクエストデータ</param>
        /// <returns>出荷先更新情報</returns>
        /// <exception cref="EntityNotFoundException">更新対象の出荷先が見つからなかった場合にスローされる</exception>
        /// <exception cref="DuplicateEntityException">出荷先名が重複している場合にスローされる</exception>
        public async Task<ShipDestinationUpdateResponse> UpdateShipDestinationAsync(
            int id,
            ShipDestinationUpdateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();

            // 入力値をトリミングしてDBの値と比較し、更新対象を検索
            var existingShipDestination = await _context.ShipDestinations
                .AsNoTracking()
                .Where(c => c.Name == trimmedInput)
                .SingleOrDefaultAsync(cancellationToken);

            if (existingShipDestination != null && existingShipDestination.Id != id)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            var shipDestination = await _context.ShipDestinations.FindAsync(id, cancellationToken);

            if (shipDestination == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // 出荷先名はリクエストの値をマッピング
            shipDestination = _mapper.Map(request, shipDestination);

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

            return _mapper.Map<ShipDestinationUpdateResponse>(shipDestination);
        }

        /// <summary>
        /// 出荷先テーブルから削除する
        /// </summary>
        /// <param name="id">出荷先ID</param>
        /// <returns>削除結果</returns>
        /// <exception cref="EntityNotFoundException">削除対象の出荷先が見つからなかった場合にスローされる</exception>
        public async Task<DeleteResponse> DeleteShipDestinationAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var shipDestination = await _context.ShipDestinations.FindAsync(id, cancellationToken);

            if (shipDestination == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            _context.ShipDestinations.Remove(shipDestination);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteResponse
            {
                Id = shipDestination.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, shipDestination.Name),
                DeletedAt = null!
            };
        }

        /// <summary>
        /// 出荷先テーブルを検索する
        /// </summary>
        /// <param name="request">出荷先検索用リクエストデータ</param>
        /// <returns>検索結果</returns>
        public async Task<SearchResponse<ShipDestinationListItemResponse>> SearchShipDestinationAsync(
            ShipDestinationSearchRequest request,
            CancellationToken cancellationToken)
        {
            // 同一トランザクション内で複数回 SELECT を実行しても、最初の時点のスナップショットを参照し続ける
            using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead, cancellationToken);

            // shipDestinationsテーブルからクエリ可能なIQueryableを取得
            var query = _context.ShipDestinations.AsNoTracking().AsQueryable();

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
                case ShipDestinationSortKey.Name:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.Name);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.Name);
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

            return new SearchResponse<ShipDestinationListItemResponse>
            {
                SearchItems = _mapper.Map<List<ShipDestinationListItemResponse>>(items),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
