using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Colors;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.Colors;
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
    /// カラーテーブルに関するサービス処理
    /// </summary>
    public class ColorService : IColorService
    {
        const string TableName = "カラー";

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ColorService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// カラーテーブルに登録する
        /// </summary>
        /// <param name="request">カラー登録用リクエストデータ</param>
        /// <returns>カラー登録情報</returns>
        /// <exception cref="DuplicateEntityException">カラーが登録済みの場合にスローされる</exception>
        public async Task<ColorCreateResponse> CreateColorAsync(
            ColorCreateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();

            // 入力値をトリミングしてDBの値と比較
            var exists = await _context.Colors
                .AsNoTracking()
                .AnyAsync(c => c.Name == trimmedInput, cancellationToken);

            // カラー名が既に登録されているかをチェック
            if (exists)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            // カラー名、各RGB値はリクエストの値をマッピング
            var color = _mapper.Map<ColorEntity>(request);
            color.Name = trimmedInput;

            _context.Colors.Add(color);

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

            return _mapper.Map<ColorCreateResponse>(color);
        }

        /// <summary>
        /// カラーテーブル詳細を取得する
        /// </summary>
        /// <param name="id">カラーID</param>
        /// <returns>カラー詳細情報</returns>
        /// <exception cref="EntityNotFoundException">カラーが見つからなかった場合にスローされる</exception>
        public async Task<ColorDetailResponse> GetColorByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var color = await _context.Colors.FindAsync(id, cancellationToken);

            if (color == null)
            {
                // エンティティが存在しない場合は、例外を投げる
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<ColorDetailResponse>(color);
        }

        /// <summary>
        /// カラーテーブルを更新する
        /// </summary>
        /// <param name="request">カラー更新用リクエストデータ</param>
        /// <returns>カラー更新情報</returns>
        /// <exception cref="EntityNotFoundException">更新対象のカラーが見つからなかった場合にスローされる</exception>
        /// <exception cref="DuplicateEntityException">カラー名が重複している場合にスローされる</exception>
        public async Task<ColorUpdateResponse> UpdateColorAsync(
            int id,
            ColorUpdateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();

            // 入力値をトリミングしてDBの値と比較し、更新対象を検索
            var existingColor = await _context.Colors
                .AsNoTracking()
                .Where(c => c.Name == trimmedInput)
                .SingleOrDefaultAsync(cancellationToken);

            if ((existingColor != null) && (existingColor.Id != id))
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            var color = await _context.Colors.FindAsync(id, cancellationToken);

            if (color == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // カラー名、各RGB値はリクエストの値をマッピング
            color = _mapper.Map(request, color);
            color.Name = trimmedInput;

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

            return _mapper.Map<ColorUpdateResponse>(color);
        }

        /// <summary>
        /// カラーテーブルから削除する
        /// </summary>
        /// <param name="id">カラーID</param>
        /// <returns>削除結果</returns>
        /// <exception cref="EntityNotFoundException">削除対象のカラーが見つからなかった場合にスローされる</exception>
        public async Task<DeleteResponse> DeleteColorAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var color = await _context.Colors.FindAsync(id, cancellationToken);

            if (color == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            _context.Colors.Remove(color);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteResponse
            {
                Id = color.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, color.Name),
                DeletedAt = null!
            };
        }

        /// <summary>
        /// カラーテーブルを検索する
        /// </summary>
        /// <param name="request">カラー検索用リクエストデータ</param>
        /// <returns>検索結果</returns>
        public async Task<SearchResponse<ColorListItemResponse>> SearchColorAsync(
            ColorSearchRequest request,
            CancellationToken cancellationToken)
        {
            // 同一トランザクション内で複数回 SELECT を実行しても、最初の時点のスナップショットを参照し続ける
            using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead, cancellationToken);

            // colorsテーブルからクエリ可能なIQueryableを取得
            var query = _context.Colors.AsNoTracking().AsQueryable();

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
                case ColorSortKey.Name:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.Name);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.Name);
                    }
                    break;
                case ColorSortKey.RedValue:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.Red);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.Red);
                    }
                    break;
                case ColorSortKey.GreenValue:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.Green);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.Green);
                    }
                    break;
                case ColorSortKey.BlueValue:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.Blue);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.Blue);
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

            return new SearchResponse<ColorListItemResponse>
            {
                SearchItems = _mapper.Map<List<ColorListItemResponse>>(items),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
