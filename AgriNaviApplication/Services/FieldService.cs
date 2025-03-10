using AgriNaviApi.Application.DTOs.Fields;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Fields;
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
    /// 圃場テーブルに関するサービス処理
    /// </summary>
    public class FieldService : IFieldService
    {
        const string TableName = "圃場";

        private readonly AppDbContext _context;
        private readonly IUuidGenerator _uuidGenerator;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public FieldService(AppDbContext context, IUuidGenerator uuidGenerator, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _uuidGenerator = uuidGenerator;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// 圃場テーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DuplicateEntityException"></exception>
        public async Task<FieldCreateDto> CreateFieldAsync(FieldCreateRequest request)
        {
            // 圃場名が既に登録されているかをチェック
            var exists = await _context.Fields.AnyAsync(u => u.Name == request.Name && !u.IsDeleted);
            if (exists)
            {
                string message = string.Format(CommonErrorMessages.DuplicateErrorMessage, request.Name);
                throw new DuplicateEntityException(message);
            }

            // 圃場名はリクエストの値をマッピング
            var field = _mapper.Map<FieldEntity>(request);

            field.Uuid = _uuidGenerator.GenerateUuid();
            field.CreatedAt = _dateTimeProvider.UtcNow;
            field.LastUpdatedAt = _dateTimeProvider.UtcNow;

            _context.Fields.Add(field);
            await _context.SaveChangesAsync();

            return _mapper.Map<FieldCreateDto>(field);
        }

        /// <summary>
        /// 圃場テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<FieldDetailDto> GetFieldByIdAsync(int id)
        {
            var field = await _context.Fields.FindAsync(id);

            if (field == null || field.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                // エンティティが存在しない場合は、例外を投げる
                throw new KeyNotFoundException(message);
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<FieldDetailDto>(field);
        }

        /// <summary>
        /// 圃場テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<FieldUpdateDto> UpdateFieldAsync(FieldUpdateRequest request)
        {
            var field = await _context.Fields.FindAsync(request.Id);

            if (field == null || field.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new KeyNotFoundException(message);
            }

            // 圃場名はリクエストの値をマッピング
            field = _mapper.Map(request, field);

            field.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<FieldUpdateDto>(field);
        }

        /// <summary>
        /// 圃場テーブルから削除する※削除フラグをtrueにするのみで、実際は削除しない
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<FieldDeleteDto> DeleteFieldAsync(FieldDeleteRequest request)
        {
            var field = await _context.Fields.FindAsync(request.Id);

            if (field == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new InvalidOperationException(message);
            }

            if (field.IsDeleted)
            {
                // 既に削除済みの場合
                string message = string.Format(CommonErrorMessages.DeletedMessage, TableName);
                throw new InvalidOperationException(message);
            }

            field.IsDeleted = true;
            field.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return new FieldDeleteDto
            {
                Id = field.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, field.Name)
            };
        }

        /// <summary>
        /// 圃場テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<FieldSearchDto> SearchFieldAsync(FieldSearchRequest request)
        {
            // fieldsテーブルからクエリ可能なIQueryableを取得
            var query = _context.Fields.AsNoTracking().AsQueryable();

            // 削除していないデータが対象
            query = query.Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchFieldName))
            {
                switch (request.SearchMatchType)
                {
                    case SearchMatchType.EXACT:
                        query = query.Where(c => c.Name == request.SearchFieldName);
                        break;
                    case SearchMatchType.PREFIX:
                        query = query.Where(c => c.Name.StartsWith(request.SearchFieldName));
                        break;
                    case SearchMatchType.SUFFIX:
                        query = query.Where(c => c.Name.EndsWith(request.SearchFieldName));
                        break;
                    case SearchMatchType.PARTIAL:
                        query = query.Where(c => c.Name.Contains(request.SearchFieldName));
                        break;
                    case SearchMatchType.None:
                    default:
                        // 全件検索とする
                        break;
                }
            }

            var fields = await query.ToListAsync();

            return new FieldSearchDto
            {
                SearchItems = _mapper.Map<List<FieldListItemDto>>(fields),
                TotalCount = fields.Count
            };
        }
    }
}
