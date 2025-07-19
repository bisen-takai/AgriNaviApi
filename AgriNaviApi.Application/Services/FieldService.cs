using AgriNaviApi.Application.Common;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Fields;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.Fields;
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
    /// 圃場テーブルに関するサービス処理
    /// </summary>
    public class FieldService : IFieldService
    {
        const string TableName = "圃場";

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IForeignKeyValidator _fkValidator;

        public FieldService(AppDbContext context, IMapper mapper, IForeignKeyValidator fkValidator)
        {
            _context = context;
            _mapper = mapper;
            _fkValidator = fkValidator;
        }

        /// <summary>
        /// 圃場テーブルに登録する
        /// </summary>
        /// <param name="request">圃場登録用リクエストデータ</param>
        /// <returns>圃場登録情報</returns>
        /// <exception cref="DuplicateEntityException">圃場が登録済みの場合にスローされる</exception>
        public async Task<FieldCreateResponse> CreateFieldAsync(
            FieldCreateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();
            var trimmedInputShortName = request.ShortName?.Trim();

            // 入力値をトリミングしてDBの値と比較
            var exists = await _context.Fields
                .AsNoTracking()
                .AnyAsync(c => c.Name == trimmedInput, cancellationToken);

            // 圃場が既に登録されているかをチェック
            if (exists)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            await _fkValidator.ValidateGroupExistsAsync(request.GroupId, cancellationToken);

            await _fkValidator.ValidateColorExistsAsync(request.ColorId, cancellationToken);

            // 余計な前後スペースを取り除いたNameでリクエストデータを更新する
            var normalizedRequest = new FieldCreateRequest
            {
                Name = trimmedInput,
                ShortName = trimmedInputShortName,
                FieldSize = request.FieldSize,
                GroupId = request.GroupId,
                ColorId = request.ColorId,
                Remark = request.Remark
            };

            // 圃場名はリクエストの値をマッピング
            var field = _mapper.Map<FieldEntity>(normalizedRequest);

            _context.Fields.Add(field);

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

            return _mapper.Map<FieldCreateResponse>(field);
        }

        /// <summary>
        /// 圃場テーブル詳細を取得する
        /// </summary>
        /// <param name="id">圃場ID</param>
        /// <returns>圃場詳細情報</returns>
        /// <exception cref="EntityNotFoundException">圃場が見つからなかった場合にスローされる</exception>
        public async Task<FieldDetailResponse> GetFieldByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var field = await _context.Fields
                .Include(f => f.Group)
                .Include(f => f.Color)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

            if (field == null)
            {
                // エンティティが存在しない場合は、例外を投げる
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<FieldDetailResponse>(field);
        }

        /// <summary>
        /// 圃場テーブルを更新する
        /// </summary>
        /// <param name="id">圃場ID</param>
        /// <param name="request">圃場更新用リクエストデータ</param>
        /// <returns>圃場更新情報</returns>
        /// <exception cref="EntityNotFoundException">更新対象の圃場が見つからなかった場合にスローされる</exception>
        /// <exception cref="DuplicateEntityException">圃場名が重複している場合にスローされる</exception>
        public async Task<FieldUpdateResponse> UpdateFieldAsync(
            int id,
            FieldUpdateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();

            // 入力値をトリミングしてDBの値と比較し、更新対象を検索
            var existingField = await _context.Fields
                .AsNoTracking()
                .Where(c => c.Name == trimmedInput)
                .SingleOrDefaultAsync(cancellationToken);

            if (existingField != null && existingField.Id != id)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            await _fkValidator.ValidateGroupExistsAsync(request.GroupId, cancellationToken);

            await _fkValidator.ValidateColorExistsAsync(request.ColorId, cancellationToken);

            var field = await _context.Fields.FindAsync(id, cancellationToken);

            if (field == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // 圃場名はリクエストの値をマッピング
            field = _mapper.Map(request, field);

            // マッピング後に Name はトリム済みの trimmedInput で上書きする
            field.Name = trimmedInput;

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

            return _mapper.Map<FieldUpdateResponse>(field);
        }

        /// <summary>
        /// 圃場テーブルから削除する
        /// </summary>
        /// <param name="id">圃場ID</param>
        /// <returns>削除結果</returns>
        /// <exception cref="EntityNotFoundException">削除対象の圃場が見つからなかった場合にスローされる</exception>
        public async Task<DeleteResponse> DeleteFieldAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var field = await _context.Fields.FindAsync(id, cancellationToken);

            if (field == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            _context.Fields.Remove(field);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteResponse
            {
                Id = field.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, field.Name),
                DeletedAt = null!
            };
        }

        /// <summary>
        /// 圃場テーブルを検索する
        /// </summary>
        /// <param name="request">圃場検索用リクエストデータ</param>
        /// <returns>検索結果</returns>
        public async Task<SearchResponse<FieldListItemResponse>> SearchFieldAsync(
            FieldSearchRequest request,
            CancellationToken cancellationToken)
        {
            // 同一トランザクション内で複数回 SELECT を実行しても、最初の時点のスナップショットを参照し続ける
            using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead, cancellationToken);

            // fieldsテーブルからクエリ可能なIQueryableを取得
            var query = _context.Fields
                .Include(f => f.Group)
                .Include(f => f.Color)
                .AsNoTracking().AsQueryable();

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
                case FieldSortKey.Name:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.Name);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.Name);
                    }
                    break;
                case FieldSortKey.Group:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.Group.Name);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.Group.Name);
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

            return new SearchResponse<FieldListItemResponse>
            {
                SearchItems = _mapper.Map<List<FieldListItemResponse>>(items),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
