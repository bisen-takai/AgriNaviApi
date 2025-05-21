using AgriNaviApi.Application.DTOs.Users;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Users;
using AgriNaviApi.Common.Enums;
using AgriNaviApi.Common.Exceptions;
using AgriNaviApi.Common.Interfaces;
using AgriNaviApi.Common.Resources;
using AgriNaviApi.Infrastructure.Persistence.Contexts;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AgriNaviApi.Application.Services
{
    /// <summary>
    /// ユーザテーブルに関するサービス処理
    /// </summary>
    public class UserService : IUserService
    {
        const string TableName = "ユーザ";

        private readonly AppDbContext _context;
        private readonly IUuidGenerator _uuidGenerator;
        private readonly ISaltGenerator _saltGenerator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UserService(AppDbContext context, IUuidGenerator uuidGenerator, ISaltGenerator saltGenerator, IPasswordHasher passwordHasher, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _uuidGenerator = uuidGenerator;
            _saltGenerator = saltGenerator;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// ユーザテーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DuplicateEntityException"></exception>
        public async Task<UserCreateDto> CreateUserAsync(UserCreateRequest request)
        {
            // ユーザIDが既に登録されているかをチェック
            var exists = await _context.Users.AnyAsync(u => u.LoginId == request.LoginId && !u.IsDeleted);
            if (exists)
            {
                string message = string.Format(CommonErrorMessages.DuplicateErrorMessage, request.LoginId);
                throw new DuplicateEntityException(message);
            }

            // ユーザ名はリクエストの値をマッピング
            var user = _mapper.Map<UserEntity>(request);

            user.Salt = _saltGenerator.GenerateSalt();
            user.PasswordHash = _passwordHasher.HashPassword(request.Password, user.Salt);
            user.Uuid = _uuidGenerator.GenerateUuid();
            user.CreatedAt = _dateTimeProvider.UtcNow;
            user.LastUpdatedAt = _dateTimeProvider.UtcNow;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserCreateDto>(user);
        }

        /// <summary>
        /// ユーザテーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<UserDetailDto> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(f => f.Color)
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (user == null || user.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                // エンティティが存在しない場合は、例外を投げる
                throw new KeyNotFoundException(message);
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<UserDetailDto>(user);
        }

        /// <summary>
        /// ユーザテーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<UserUpdateDto> UpdateUserAsync(UserUpdateRequest request)
        {
            var user = await _context.Users.FindAsync(request.Id);

            if (user == null || user.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new KeyNotFoundException(message);
            }

            // ユーザ名はリクエストの値をマッピング
            user = _mapper.Map(request, user);

            user.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<UserUpdateDto>(user);
        }

        /// <summary>
        /// ユーザパスワードを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<UserUpdateDto> UpdateUserPasswordAsync(PasswordUpdateRequest request)
        {
            var user = await _context.Users.FindAsync(request.Id);

            if (user == null || user.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new KeyNotFoundException(message);
            }

            user.Salt = _saltGenerator.GenerateSalt();
            user.PasswordHash = _passwordHasher.HashPassword(request.Password, user.Salt);

            user.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<UserUpdateDto>(user);
        }

        /// <summary>
        /// ユーザテーブルから削除する※削除フラグをtrueにするのみで、実際は削除しない
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<UserDeleteDto> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new InvalidOperationException(message);
            }

            if (user.IsDeleted)
            {
                // 既に削除済みの場合
                string message = string.Format(CommonErrorMessages.DeletedMessage, TableName);
                throw new InvalidOperationException(message);
            }

            user.IsDeleted = true;
            user.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return new UserDeleteDto
            {
                Id = user.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, user.FullName)
            };
        }

        /// <summary>
        /// ユーザテーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UserSearchDto> SearchUserAsync(UserSearchRequest request)
        {
            // usersテーブルからクエリ可能なIQueryableを取得
            var query = _context.Users
                .Include(f => f.Color)
                .AsNoTracking().AsQueryable();

            // 削除していないデータが対象
            query = query.Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchUserName))
            {
                switch (request.SearchMatchType)
                {
                    case SearchMatchType.EXACT:
                        query = query.Where(c => c.FullName == request.SearchUserName);
                        break;
                    case SearchMatchType.PREFIX:
                        query = query.Where(c => c.FullName.StartsWith(request.SearchUserName));
                        break;
                    case SearchMatchType.SUFFIX:
                        query = query.Where(c => c.FullName.EndsWith(request.SearchUserName));
                        break;
                    case SearchMatchType.PARTIAL:
                        query = query.Where(c => c.FullName.Contains(request.SearchUserName));
                        break;
                    case SearchMatchType.None:
                    default:
                        // 全件検索とする
                        break;
                }
            }

            var users = await query.ToListAsync();

            return new UserSearchDto
            {
                SearchItems = _mapper.Map<List<UserListItemDto>>(users),
                TotalCount = users.Count
            };
        }

        /// <summary>
        /// ログインを行う
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<UserLoginDto> AuthenticateAsync(UserLoginRequest request)
        {
            var user = await _context.Users
                .Where(u => u.LoginId == request.LoginId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new InvalidOperationException(UserErrorMessages.LoginIdErrorMessage);
            }

            if (user.IsDeleted)
            {
                // 既に削除済みの場合
                string message = string.Format(CommonErrorMessages.DeletedMessage, TableName);
                throw new InvalidOperationException(message);
            }

            var hashedPassword = _passwordHasher.HashPassword(request.Password, user.Salt);
            if (user.PasswordHash != hashedPassword)
            {
                throw new InvalidOperationException(UserErrorMessages.PasswordErrorMessage);
            }

            return _mapper.Map<UserLoginDto>(user);
        }
    }
}
