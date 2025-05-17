using AgriNaviApi.Application.DTOs.QualityStandards;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.QualityStandards;
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
    /// 品質・規格テーブルに関するサービス処理
    /// </summary>
    public class QualityStandardService : IQualityStandardService
    {
        const string TableName = "品質・規格";

        private readonly AppDbContext _context;
        private readonly IUuidGenerator _uuidGenerator;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public QualityStandardService(AppDbContext context, IUuidGenerator uuidGenerator, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _uuidGenerator = uuidGenerator;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// 品質・規格テーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DuplicateEntityException"></exception>
        public async Task<QualityStandardCreateDto> CreateQualityStandardAsync(QualityStandardCreateRequest request)
        {
            // 品質・規格名が既に登録されているかをチェック
            var exists = await _context.QualityStandards.AnyAsync(u => u.Name == request.Name && !u.IsDeleted);
            if (exists)
            {
                string message = string.Format(CommonErrorMessages.DuplicateErrorMessage, request.Name);
                throw new DuplicateEntityException(message);
            }

            // 品質・規格名はリクエストの値をマッピング
            var qualityStandard = _mapper.Map<QualityStandardEntity>(request);

            qualityStandard.Uuid = _uuidGenerator.GenerateUuid();
            qualityStandard.CreatedAt = _dateTimeProvider.UtcNow;
            qualityStandard.LastUpdatedAt = _dateTimeProvider.UtcNow;

            _context.QualityStandards.Add(qualityStandard);
            await _context.SaveChangesAsync();

            return _mapper.Map<QualityStandardCreateDto>(qualityStandard);
        }

        /// <summary>
        /// 品質・規格テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<QualityStandardDetailDto> GetQualityStandardByIdAsync(int id)
        {
            var qualityStandard = await _context.QualityStandards.FindAsync(id);

            if (qualityStandard == null || qualityStandard.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                // エンティティが存在しない場合は、例外を投げる
                throw new KeyNotFoundException(message);
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<QualityStandardDetailDto>(qualityStandard);
        }

        /// <summary>
        /// 品質・規格テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<QualityStandardUpdateDto> UpdateQualityStandardAsync(QualityStandardUpdateRequest request)
        {
            var qualityStandard = await _context.QualityStandards.FindAsync(request.Id);

            if (qualityStandard == null || qualityStandard.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new KeyNotFoundException(message);
            }

            // 品質・規格名はリクエストの値をマッピング
            qualityStandard = _mapper.Map(request, qualityStandard);

            qualityStandard.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<QualityStandardUpdateDto>(qualityStandard);
        }

        /// <summary>
        /// 品質・規格テーブルから削除する※削除フラグをtrueにするのみで、実際は削除しない
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<QualityStandardDeleteDto> DeleteQualityStandardAsync(int id)
        {
            var qualityStandard = await _context.QualityStandards.FindAsync(id);

            if (qualityStandard == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new InvalidOperationException(message);
            }

            if (qualityStandard.IsDeleted)
            {
                // 既に削除済みの場合
                string message = string.Format(CommonErrorMessages.DeletedMessage, TableName);
                throw new InvalidOperationException(message);
            }

            qualityStandard.IsDeleted = true;
            qualityStandard.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return new QualityStandardDeleteDto
            {
                Id = qualityStandard.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, qualityStandard.Name)
            };
        }

        /// <summary>
        /// 品質・規格テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<QualityStandardSearchDto> SearchQualityStandardAsync(QualityStandardSearchRequest request)
        {
            // qualityStandardsテーブルからクエリ可能なIQueryableを取得
            var query = _context.QualityStandards.AsNoTracking().AsQueryable();

            // 削除していないデータが対象
            query = query.Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchQualityStandardName))
            {
                switch (request.SearchMatchType)
                {
                    case SearchMatchType.EXACT:
                        query = query.Where(c => c.Name == request.SearchQualityStandardName);
                        break;
                    case SearchMatchType.PREFIX:
                        query = query.Where(c => c.Name.StartsWith(request.SearchQualityStandardName));
                        break;
                    case SearchMatchType.SUFFIX:
                        query = query.Where(c => c.Name.EndsWith(request.SearchQualityStandardName));
                        break;
                    case SearchMatchType.PARTIAL:
                        query = query.Where(c => c.Name.Contains(request.SearchQualityStandardName));
                        break;
                    case SearchMatchType.None:
                    default:
                        // 全件検索とする
                        break;
                }
            }

            var qualityStandards = await query.ToListAsync();

            return new QualityStandardSearchDto
            {
                SearchItems = _mapper.Map<List<QualityStandardListItemDto>>(qualityStandards),
                TotalCount = qualityStandards.Count
            };
        }
    }
}
