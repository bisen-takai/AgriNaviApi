using AgriNaviApi.Application.DTOs.Groups;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Groups;
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
    /// グループテーブルに関するサービス処理
    /// </summary>
    public class GroupService : IGroupService
    {
        const string TableName = "グループ";

        private readonly AppDbContext _context;
        private readonly IUuidGenerator _uuidGenerator;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GroupService(AppDbContext context, IUuidGenerator uuidGenerator, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _uuidGenerator = uuidGenerator;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// グループテーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DuplicateEntityException"></exception>
        public async Task<GroupCreateDto> CreateGroupAsync(GroupCreateRequest request)
        {
            // グループ名が既に登録されているかをチェック
            var exists = await _context.Groups.AnyAsync(u => u.Kind == request.Kind && u.Name == request.Name && !u.IsDeleted);
            if (exists)
            {
                string message = string.Format(CommonErrorMessages.DuplicateErrorMessage, request.Name);
                throw new DuplicateEntityException(message);
            }

            // グループ名はリクエストの値をマッピング
            var group = _mapper.Map<GroupEntity>(request);

            group.Uuid = _uuidGenerator.GenerateUuid();
            group.CreatedAt = _dateTimeProvider.UtcNow;
            group.LastUpdatedAt = _dateTimeProvider.UtcNow;

            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return _mapper.Map<GroupCreateDto>(group);
        }

        /// <summary>
        /// グループテーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<GroupDetailDto> GetGroupByIdAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);

            if (group == null || group.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                // エンティティが存在しない場合は、例外を投げる
                throw new KeyNotFoundException(message);
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<GroupDetailDto>(group);
        }

        /// <summary>
        /// グループテーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<GroupUpdateDto> UpdateGroupAsync(GroupUpdateRequest request)
        {
            var group = await _context.Groups.FindAsync(request.Id);

            if (group == null || group.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new KeyNotFoundException(message);
            }

            // グループ名はリクエストの値をマッピング
            group = _mapper.Map(request, group);

            group.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<GroupUpdateDto>(group);
        }

        /// <summary>
        /// グループテーブルから削除する※削除フラグをtrueにするのみで、実際は削除しない
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<GroupDeleteDto> DeleteGroupAsync(GroupDeleteRequest request)
        {
            var group = await _context.Groups.FindAsync(request.Id);

            if (group == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new InvalidOperationException(message);
            }

            if (group.IsDeleted)
            {
                // 既に削除済みの場合
                string message = string.Format(CommonErrorMessages.DeletedMessage, TableName);
                throw new InvalidOperationException(message);
            }

            group.IsDeleted = true;
            group.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return new GroupDeleteDto
            {
                Id = group.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, group.Name)
            };
        }

        /// <summary>
        /// グループテーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GroupSearchDto> SearchGroupAsync(GroupSearchRequest request)
        {
            // groupsテーブルからクエリ可能なIQueryableを取得
            var query = _context.Groups.AsNoTracking().AsQueryable();

            // 削除していないデータが対象
            query = query.Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchGroupName))
            {
                switch (request.SearchMatchType)
                {
                    case SearchMatchType.EXACT:
                        query = query.Where(c => c.Kind == request.Kind && c.Name == request.SearchGroupName);
                        break;
                    case SearchMatchType.PREFIX:
                        query = query.Where(c => c.Kind == request.Kind && c.Name.StartsWith(request.SearchGroupName));
                        break;
                    case SearchMatchType.SUFFIX:
                        query = query.Where(c => c.Kind == request.Kind && c.Name.EndsWith(request.SearchGroupName));
                        break;
                    case SearchMatchType.PARTIAL:
                        query = query.Where(c => c.Kind == request.Kind && c.Name.Contains(request.SearchGroupName));
                        break;
                    case SearchMatchType.None:
                    default:
                        // 全件検索とする
                        query = query.Where(c => c.Kind == request.Kind);
                        break;
                }
            }

            var groups = await query.ToListAsync();

            return new GroupSearchDto
            {
                SearchItems = _mapper.Map<List<GroupListItemDto>>(groups),
                TotalCount = groups.Count
            };
        }
    }
}
