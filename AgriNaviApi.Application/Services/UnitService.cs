using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Units;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.Units;
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
    /// 単位テーブルに関するサービス処理
    /// </summary>
    public class UnitService : IUnitService
    {
        const string TableName = "単位";

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UnitService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// 単位テーブルに登録する
        /// </summary>
        /// <param name="request">単位登録用リクエストデータ</param>
        /// <returns>単位登録情報</returns>
        /// <exception cref="DuplicateEntityException">単位が登録済みの場合にスローされる</exception>
        public async Task<UnitCreateResponse> CreateUnitAsync(
            UnitCreateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();

            // 入力値をトリミングしてDBの値と比較
            var exists = await _context.Units
                .AsNoTracking()
                .AnyAsync(c => c.Name == trimmedInput, cancellationToken);

            // 単位が既に登録されているかをチェック
            if (exists)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            // 単位名はリクエストの値をマッピング
            var unit = _mapper.Map<UnitEntity>(request);

            _context.Units.Add(unit);

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

            return _mapper.Map<UnitCreateResponse>(unit);
        }

        /// <summary>
        /// 単位テーブル詳細を取得する
        /// </summary>
        /// <param name="id">単位ID</param>
        /// <returns>単位詳細情報</returns>
        /// <exception cref="EntityNotFoundException">単位が見つからなかった場合にスローされる</exception>
        public async Task<UnitDetailResponse> GetUnitByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var unit = await _context.Units.FindAsync(id, cancellationToken);

            if (unit == null)
            {
                // エンティティが存在しない場合は、例外を投げる
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<UnitDetailResponse>(unit);
        }

        /// <summary>
        /// 単位テーブルを更新する
        /// </summary>
        /// <param name="id">単位ID</param>
        /// <param name="request">単位更新用リクエストデータ</param>
        /// <returns>単位更新情報</returns>
        /// <exception cref="EntityNotFoundException">更新対象の単位が見つからなかった場合にスローされる</exception>
        /// <exception cref="DuplicateEntityException">単位名が重複している場合にスローされる</exception>
        public async Task<UnitUpdateResponse> UpdateUnitAsync(
            int id,
            UnitUpdateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();

            // 入力値をトリミングしてDBの値と比較し、更新対象を検索
            var existingUnit = await _context.Units
                .AsNoTracking()
                .Where(c => c.Name == trimmedInput)
                .SingleOrDefaultAsync(cancellationToken);

            if (existingUnit != null && existingUnit.Id != id)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            var unit = await _context.Units.FindAsync(id, cancellationToken);

            if (unit == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // 単位名はリクエストの値をマッピング
            unit = _mapper.Map(request, unit);

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

            return _mapper.Map<UnitUpdateResponse>(unit);
        }

        /// <summary>
        /// 単位テーブルから削除する
        /// </summary>
        /// <param name="id">単位ID</param>
        /// <returns>削除結果</returns>
        /// <exception cref="EntityNotFoundException">削除対象の単位が見つからなかった場合にスローされる</exception>
        public async Task<DeleteResponse> DeleteUnitAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var unit = await _context.Units.FindAsync(id, cancellationToken);

            if (unit == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            _context.Units.Remove(unit);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteResponse
            {
                Id = unit.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, unit.Name),
                DeletedAt = null!
            };
        }

        /// <summary>
        /// 単位テーブルを検索する
        /// </summary>
        /// <param name="request">単位検索用リクエストデータ</param>
        /// <returns>検索結果</returns>
        public async Task<SearchResponse<UnitListItemResponse>> SearchUnitAsync(
            UnitSearchRequest request,
            CancellationToken cancellationToken)
        {
            // 同一トランザクション内で複数回 SELECT を実行しても、最初の時点のスナップショットを参照し続ける
            using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead, cancellationToken);

            // unitsテーブルからクエリ可能なIQueryableを取得
            var query = _context.Units.AsNoTracking().AsQueryable();

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
                case UnitSortKey.Name:
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

            return new SearchResponse<UnitListItemResponse>
            {
                SearchItems = _mapper.Map<List<UnitListItemResponse>>(items),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
