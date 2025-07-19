using AgriNaviApi.Application.Common;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Users;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.Users;
using AgriNaviApi.Infrastructure.Persistence.Contexts;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AgriNaviApi.Shared.Enums;
using AgriNaviApi.Shared.Exceptions;
using AgriNaviApi.Shared.Interfaces;
using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.Utilities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace AgriNaviApi.Application.Services
{
    /// <summary>
    /// ユーザテーブルに関するサービス処理
    /// </summary>
    public class UserService : IUserService
    {
        const string TableName = "ユーザ";

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ISaltGenerator _saltGenerator;
        private readonly IForeignKeyValidator _fkValidator;

        public UserService(AppDbContext context, IMapper mapper, IForeignKeyValidator fkValidator, IPasswordHasher passwordHasher, ISaltGenerator saltGenerator)
        {
            _context = context;
            _mapper = mapper;
            _fkValidator = fkValidator;
            _passwordHasher = passwordHasher;
            _saltGenerator = saltGenerator;
        }

        /// <summary>
        /// ユーザテーブルに登録する
        /// </summary>
        /// <param name="request">ユーザ登録用リクエストデータ</param>
        /// <returns>ユーザ登録情報</returns>
        /// <exception cref="DuplicateEntityException">ユーザが登録済みの場合にスローされる</exception>
        public async Task<UserCreateResponse> CreateUserAsync(
            UserCreateRequest request,
            CancellationToken cancellationToken)
        {
            // 入力値をトリミングしてDBの値と比較
            var exists = await _context.Users
                .AsNoTracking()
                .AnyAsync(c => c.LoginId == request.LoginId, cancellationToken);

            // ユーザが既に登録されているかをチェック
            if (exists)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.LoginId }
                );
            }

            await _fkValidator.ValidateColorExistsAsync(request.ColorId, cancellationToken);

            // ユーザ名はリクエストの値をマッピング
            var user = _mapper.Map<UserEntity>(request);

            user.Salt = _saltGenerator.GenerateSalt();
            user.PasswordHash = _passwordHasher.HashPassword(request.Password, user.Salt);

            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (DbExceptionHelper.IsUniqueConstraintViolation(ex))
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.LoginId }
                );
            }

            return _mapper.Map<UserCreateResponse>(user);
        }

        /// <summary>
        /// ユーザテーブル詳細を取得する
        /// </summary>
        /// <param name="id">ユーザID</param>
        /// <returns>ユーザ詳細情報</returns>
        /// <exception cref="EntityNotFoundException">ユーザが見つからなかった場合にスローされる</exception>
        public async Task<UserDetailResponse> GetUserByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(f => f.Color)
                .Where(s => !s.IsDeleted)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Uuid == id, cancellationToken);

            if (user == null)
            {
                // エンティティが存在しない場合は、例外を投げる
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<UserDetailResponse>(user);
        }

        /// <summary>
        /// ユーザテーブルを更新する
        /// </summary>
        /// <param name="id">ユーザID</param>
        /// <param name="request">ユーザ更新用リクエストデータ</param>
        /// <returns>ユーザ更新情報</returns>
        /// <exception cref="EntityNotFoundException">更新対象のユーザが見つからなかった場合にスローされる</exception>
        /// <exception cref="DuplicateEntityException">ユーザ名が重複している場合にスローされる</exception>
        public async Task<UserUpdateResponse> UpdateUserAsync(
            Guid id,
            UserUpdateRequest request,
            CancellationToken cancellationToken)
        {
            // 入力値をトリミングしてDBの値と比較し、更新対象を検索
            var user = await _context.Users
                .AsNoTracking()
                .Where(c => c.Uuid == id && !c.IsDeleted)
                .SingleOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            bool isDuplicateLoginId = await _context.Users
                .AsNoTracking()
                .AnyAsync(c => c.LoginId == request.LoginId && c.Uuid != id, cancellationToken);

            if (isDuplicateLoginId)
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.LoginId }
                );
            }

            await _fkValidator.ValidateColorExistsAsync(request.ColorId, cancellationToken);

            // ユーザ名はリクエストの値をマッピング
            user = _mapper.Map(request, user);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (DbExceptionHelper.IsUniqueConstraintViolation(ex))
            {
                throw new DuplicateEntityException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.DuplicateErrorMessage),
                    args: new[] { request.LoginId }
                );
            }

            return _mapper.Map<UserUpdateResponse>(user);
        }

        /// <summary>
        /// ユーザパスワードを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<PasswordUpdateResponse> UpdateUserPasswordAsync(
            Guid id,
            PasswordUpdateRequest request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(f => f.Uuid == id, cancellationToken);

            if (user == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            user.Salt = _saltGenerator.GenerateSalt();
            user.PasswordHash = _passwordHasher.HashPassword(request.Password, user.Salt);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PasswordUpdateResponse>(user);
        }

        /// <summary>
        /// ユーザテーブルから削除する
        /// </summary>
        /// <param name="id">ユーザID</param>
        /// <returns>削除結果</returns>
        /// <exception cref="EntityNotFoundException">削除対象のユーザが見つからなかった場合にスローされる</exception>
        public async Task<DeleteWithUuidResponse> DeleteUserAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(s => !s.IsDeleted)
                .FirstOrDefaultAsync(f => f.Uuid == id, cancellationToken);

            if (user == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.NotFoundErrorMessage),
                    args: new[] { TableName }
                );
            }

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteWithUuidResponse
            {
                Uuid = user.Uuid,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, user.LoginId),
                DeletedAt = user.DeletedAt
            };
        }

        /// <summary>
        /// ユーザテーブルを検索する
        /// </summary>
        /// <param name="request">ユーザ検索用リクエストデータ</param>
        /// <returns>検索結果</returns>
        public async Task<SearchResponse<UserListItemResponse>> SearchUserAsync(
            UserSearchRequest request,
            CancellationToken cancellationToken)
        {
            // 同一トランザクション内で複数回 SELECT を実行しても、最初の時点のスナップショットを参照し続ける
            using var tx = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.RepeatableRead, cancellationToken);

            // Usersテーブルからクエリ可能なIQueryableを取得
            var query = _context.Users
                .Include(f => f.Color)
                .Where(u => !u.IsDeleted)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchName))
            {
                // 余計な前後スペースだけ取り除く
                var trimmedSearch = request.SearchName.Trim();

                switch (request.SearchMatchType)
                {
                    case SearchMatchType.Exact:
                        query = query.Where(c => c.FullName == trimmedSearch);
                        break;
                    case SearchMatchType.Prefix:
                        query = query.Where(c => c.FullName.StartsWith(trimmedSearch));
                        break;
                    case SearchMatchType.Suffix:
                        query = query.Where(c => c.FullName.EndsWith(trimmedSearch));
                        break;
                    case SearchMatchType.Partial:
                        query = query.Where(c => c.FullName.Contains(trimmedSearch));
                        break;
                    case SearchMatchType.None:
                    default:
                        // 全件検索とする
                        break;
                }
            }

            switch (request.SortBy)
            {
                case UserSortKey.LoginId:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.LoginId);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.LoginId);
                    }
                    break;
                case UserSortKey.PrivilegeId:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.PrivilegeId);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.PrivilegeId);
                    }
                    break;
                case UserSortKey.FullName:
                    if (request.SortDescending)
                    {
                        query = query.OrderByDescending(c => c.FullName);
                    }
                    else
                    {
                        query = query.OrderBy(c => c.FullName);
                    }
                    break;
                default:
                    query = query.OrderBy(c => c.FullName);
                    break;
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            // トランザクションは読み取り専用なのでコミットだけ
            await tx.CommitAsync(cancellationToken);

            return new SearchResponse<UserListItemResponse>
            {
                SearchItems = _mapper.Map<List<UserListItemResponse>>(items),
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }

        /// <summary>
        /// ログインを行う
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<UserLoginResponse> AuthenticateAsync(
            UserLoginRequest request,
            CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Where(u => u.LoginId == request.LoginId && !u.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(UserErrorMessages),
                    resourceKey: nameof(UserErrorMessages.LoginIdErrorMessage),
                    args: new[] { request.LoginId }
                );
            }

            var hashedPassword = _passwordHasher.HashPassword(request.Password, user.Salt);

            if (user.PasswordHash != hashedPassword)
            {
                throw new EntityNotFoundException(
                    resourceType: typeof(UserErrorMessages),
                    resourceKey: nameof(UserErrorMessages.PasswordErrorMessage),
                    args: new[] { request.LoginId }
                );
            }

            return _mapper.Map<UserLoginResponse>(user);
        }
    }
}
