using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.QualityStandards;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.QualityStandards;
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
    /// 品質・規格テーブルに関するサービス処理
    /// </summary>
    public class QualityStandardService : IQualityStandardService
    {
        const string TableName = "品質・規格";

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public QualityStandardService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// 品質・規格テーブルに登録する
        /// </summary>
        /// <param name="request">品質・規格登録用リクエストデータ</param>
        /// <returns>品質・規格登録情報</returns>
        /// <exception cref="DuplicateEntityException">品質・規格が登録済みの場合にスローされる</exception>
        public async Task<QualityStandardCreateResponse> CreateQualityStandardAsync(
            QualityStandardCreateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();

            // 入力値をトリミングしてDBの値と比較
            var exists = await _context.QualityStandards
                .AsNoTracking()
                .AnyAsync(c => c.Name == trimmedInput, cancellationToken);

            // 品質・規格が既に登録されているかをチェック
            if (exists)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            // 品質・規格名はリクエストの値をマッピング
            var qualityStandard = _mapper.Map<QualityStandardEntity>(request);

            _context.QualityStandards.Add(qualityStandard);

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

            return _mapper.Map<QualityStandardCreateResponse>(qualityStandard);
        }

        /// <summary>
        /// 品質・規格テーブル詳細を取得する
        /// </summary>
        /// <param name="id">品質・規格ID</param>
        /// <returns>品質・規格詳細情報</returns>
        /// <exception cref="EntityNotFoundException">品質・規格が見つからなかった場合にスローされる</exception>
        public async Task<QualityStandardDetailResponse> GetQualityStandardByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var qualityStandard = await _context.QualityStandards.FindAsync(id, cancellationToken);

            if (qualityStandard == null)
            {
                // エンティティが存在しない場合は、例外を投げる
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<QualityStandardDetailResponse>(qualityStandard);
        }

        /// <summary>
        /// 品質・規格テーブルを更新する
        /// </summary>
        /// <param name="id">品質・規格ID</param>
        /// <param name="request">品質・規格更新用リクエストデータ</param>
        /// <returns>品質・規格更新情報</returns>
        /// <exception cref="EntityNotFoundException">更新対象の品質・規格が見つからなかった場合にスローされる</exception>
        /// <exception cref="DuplicateEntityException">品質・規格名が重複している場合にスローされる</exception>
        public async Task<QualityStandardUpdateResponse> UpdateQualityStandardAsync(
            int id,
            QualityStandardUpdateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();

            // 入力値をトリミングしてDBの値と比較し、更新対象を検索
            var existingQualityStandard = await _context.QualityStandards
                .AsNoTracking()
                .Where(c => c.Name == trimmedInput)
                .SingleOrDefaultAsync(cancellationToken);

            if (existingQualityStandard != null && existingQualityStandard.Id != id)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            var qualityStandard = await _context.QualityStandards.FindAsync(id, cancellationToken);

            if (qualityStandard == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // 品質・規格名はリクエストの値をマッピング
            qualityStandard = _mapper.Map(request, qualityStandard);

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

            return _mapper.Map<QualityStandardUpdateResponse>(qualityStandard);
        }

        /// <summary>
        /// 品質・規格テーブルから削除する
        /// </summary>
        /// <param name="id">品質・規格ID</param>
        /// <returns>削除結果</returns>
        /// <exception cref="EntityNotFoundException">削除対象の品質・規格が見つからなかった場合にスローされる</exception>
        public async Task<DeleteResponse> DeleteQualityStandardAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var qualityStandard = await _context.QualityStandards.FindAsync(id, cancellationToken);

            if (qualityStandard == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            _context.QualityStandards.Remove(qualityStandard);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteResponse
            {
                Id = qualityStandard.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, qualityStandard.Name),
                DeletedAt = null!
            };
        }

        /// <summary>
        /// 品質・規格テーブルを検索する
        /// </summary>
        /// <param name="request">品質・規格検索用リクエストデータ</param>
        /// <returns>検索結果</returns>
        public async Task<SearchResponse<QualityStandardListItemResponse>> SearchQualityStandardAsync(
            QualityStandardSearchRequest request,
            CancellationToken cancellationToken)
        {
            // 同一トランザクション内で複数回 SELECT を実行しても、最初の時点のスナップショットを参照し続ける
            using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead, cancellationToken);

            // qualityStandardsテーブルからクエリ可能なIQueryableを取得
            var query = _context.QualityStandards.AsNoTracking().AsQueryable();

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
                case QualityStandardSortKey.Name:
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

            return new SearchResponse<QualityStandardListItemResponse>
            {
                SearchItems = _mapper.Map<List<QualityStandardListItemResponse>>(items),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
