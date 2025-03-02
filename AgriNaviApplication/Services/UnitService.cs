using AgriNaviApi.Application.DTOs.Units;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Units;
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
    /// 単位テーブルに関するサービス処理
    /// </summary>
    public class UnitService : IUnitService
    {
        const string TableName = "単位";

        private readonly AppDbContext _context;
        private readonly IUuidGenerator _uuidGenerator;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UnitService(AppDbContext context, IUuidGenerator uuidGenerator, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _uuidGenerator = uuidGenerator;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// 単位テーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DuplicateEntityException"></exception>
        public async Task<UnitCreateDto> CreateUnitAsync(UnitCreateRequest request)
        {
            // 単位名が既に登録されているかをチェック
            var exists = await _context.Units.AnyAsync(u => u.Name == request.Name);
            if (exists)
            {
                string message = string.Format(CommonErrorMessages.DuplicateErrorMessage, request.Name);
                throw new DuplicateEntityException(message);
            }

            // 単位名はリクエストの値をマッピング
            var unit = _mapper.Map<UnitEntity>(request);

            unit.Uuid = _uuidGenerator.GenerateUuid();
            unit.CreatedAt = _dateTimeProvider.UtcNow;
            unit.LastUpdatedAt = _dateTimeProvider.UtcNow;

            _context.Units.Add(unit);
            await _context.SaveChangesAsync();

            return _mapper.Map<UnitCreateDto>(unit);
        }

        /// <summary>
        /// 単位テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<UnitDetailDto> GetUnitByIdAsync(int id)
        {
            var unit = await _context.Units.FindAsync(id);

            if (unit == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                // エンティティが存在しない場合は、例外を投げる
                throw new KeyNotFoundException(message);
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<UnitDetailDto>(unit);
        }

        /// <summary>
        /// 単位テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<UnitUpdateDto> UpdateUnitAsync(UnitUpdateRequest request)
        {
            var unit = await _context.Units.FindAsync(request.Id);

            if (unit == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new KeyNotFoundException(message);
            }

            // 単位名はリクエストの値をマッピング
            unit = _mapper.Map(request, unit);

            unit.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<UnitUpdateDto>(unit);
        }

        /// <summary>
        /// 単位テーブルから削除する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<UnitDeleteDto> DeleteUnitAsync(UnitDeleteRequest request)
        {
            var unit = await _context.Units.FindAsync(request.Id);

            if (unit == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new InvalidOperationException(message);
            }

            _context.Units.Remove(unit);
            await _context.SaveChangesAsync();

            return new UnitDeleteDto
            {
                Id = unit.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, unit.Name)
            };
        }

        /// <summary>
        /// 単位テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UnitSearchDto> SearchUnitAsync(UnitSearchRequest request)
        {
            // unitsテーブルからクエリ可能なIQueryableを取得
            var query = _context.Units.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchUnitName))
            {
                switch (request.SearchMatchType)
                {
                    case SearchMatchType.EXACT:
                        query = query.Where(c => c.Name == request.SearchUnitName);
                        break;
                    case SearchMatchType.PREFIX:
                        query = query.Where(c => c.Name.StartsWith(request.SearchUnitName));
                        break;
                    case SearchMatchType.SUFFIX:
                        query = query.Where(c => c.Name.EndsWith(request.SearchUnitName));
                        break;
                    case SearchMatchType.PARTIAL:
                        query = query.Where(c => c.Name.Contains(request.SearchUnitName));
                        break;
                    case SearchMatchType.None:
                    default:
                        // 全件検索とする
                        break;
                }
            }

            var units = await query.ToListAsync();

            return new UnitSearchDto
            {
                SearchItems = _mapper.Map<List<UnitListItemDto>>(units),
                TotalCount = units.Count
            };
        }
    }
}
