using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Groups;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.Groups;
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
    /// グループテーブルに関するサービス処理
    /// </summary>
    public class GroupService : IGroupService
    {
        const string TableName = "グループ";

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GroupService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// グループテーブルに登録する
        /// </summary>
        /// <param name="request">グループ登録用リクエストデータ</param>
        /// <returns>グループ登録情報</returns>
        /// <exception cref="DuplicateEntityException">グループが登録済みの場合にスローされる</exception>
        public async Task<GroupCreateResponse> CreateGroupAsync(
            GroupCreateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();

            // 入力値をトリミングしてDBの値と比較
            var exists = await _context.Groups
                .AsNoTracking()
                .AnyAsync(c => c.Name == trimmedInput, cancellationToken);

            // グループが既に登録されているかをチェック
            if (exists)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            // グループ名はリクエストの値をマッピング
            var group = _mapper.Map<GroupEntity>(request);

            _context.Groups.Add(group);

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

            return _mapper.Map<GroupCreateResponse>(group);
        }

        /// <summary>
        /// グループテーブル詳細を取得する
        /// </summary>
        /// <param name="id">グループID</param>
        /// <returns>グループ詳細情報</returns>
        /// <exception cref="EntityNotFoundException">グループが見つからなかった場合にスローされる</exception>
        public async Task<GroupDetailResponse> GetGroupByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var group = await _context.Groups.FindAsync(id, cancellationToken);

            if (group == null)
            {
                // エンティティが存在しない場合は、例外を投げる
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<GroupDetailResponse>(group);
        }

        /// <summary>
        /// グループテーブルを更新する
        /// </summary>
        /// <param name="id">グループID</param>
        /// <param name="request">グループ更新用リクエストデータ</param>
        /// <returns>グループ更新情報</returns>
        /// <exception cref="EntityNotFoundException">更新対象のグループが見つからなかった場合にスローされる</exception>
        /// <exception cref="DuplicateEntityException">グループ名が重複している場合にスローされる</exception>
        public async Task<GroupUpdateResponse> UpdateGroupAsync(
            int id,
            GroupUpdateRequest request,
            CancellationToken cancellationToken)
        {
            // 余計な前後スペースだけ取り除く
            var trimmedInput = request.Name.Trim();

            // 入力値をトリミングしてDBの値と比較し、更新対象を検索
            var existingGroup = await _context.Groups
                .AsNoTracking()
                .Where(c => c.Name == trimmedInput)
                .SingleOrDefaultAsync(cancellationToken);

            if (existingGroup != null && existingGroup.Id != id)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.Name }
                );
            }

            var group = await _context.Groups.FindAsync(id, cancellationToken);

            if (group == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // グループ名はリクエストの値をマッピング
            group = _mapper.Map(request, group);

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

            return _mapper.Map<GroupUpdateResponse>(group);
        }

        /// <summary>
        /// グループテーブルから削除する
        /// </summary>
        /// <param name="id">グループID</param>
        /// <returns>削除結果</returns>
        /// <exception cref="EntityNotFoundException">削除対象のグループが見つからなかった場合にスローされる</exception>
        public async Task<DeleteResponse> DeleteGroupAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var group = await _context.Groups.FindAsync(id, cancellationToken);

            if (group == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteResponse
            {
                Id = group.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, group.Name),
                DeletedAt = null!
            };
        }

        /// <summary>
        /// グループテーブルを検索する
        /// </summary>
        /// <param name="request">グループ検索用リクエストデータ</param>
        /// <returns>検索結果</returns>
        public async Task<SearchResponse<GroupListItemResponse>> SearchGroupAsync(
            GroupSearchRequest request,
            CancellationToken cancellationToken)
        {
            // 同一トランザクション内で複数回 SELECT を実行しても、最初の時点のスナップショットを参照し続ける
            using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead, cancellationToken);

            // groupsテーブルからクエリ可能なIQueryableを取得
            var query = _context.Groups.AsNoTracking().AsQueryable();

            query = query.Where(c => c.Kind == request.Kind);

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
                case GroupSortKey.Name:
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

            return new SearchResponse<GroupListItemResponse>
            {
                SearchItems = _mapper.Map<List<GroupListItemResponse>>(items),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}
