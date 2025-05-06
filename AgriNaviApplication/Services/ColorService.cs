using AgriNaviApi.Application.DTOs.Colors;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Colors;
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
    /// カラーテーブルに関するサービス処理
    /// </summary>
    public class ColorService : IColorService
    {
        const string TableName = "カラー";

        private readonly AppDbContext _context;
        private readonly IUuidGenerator _uuidGenerator;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ColorService(AppDbContext context, IUuidGenerator uuidGenerator, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _uuidGenerator = uuidGenerator;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// カラーテーブルに登録する
        /// </summary>
        /// <param name="request">登録用リクエストデータ</param>
        /// <returns></returns>
        public async Task<ColorCreateDto> CreateColorAsync(ColorCreateRequest request)
        {
            // カラー名が既に登録されているかをチェック
            var exists = await _context.Colors.AnyAsync(u => u.Name == request.Name);
            if (exists)
            {
                string message = string.Format(CommonErrorMessages.DuplicateErrorMessage, request.Name);
                throw new DuplicateEntityException(message);
            }

            // カラー名、各RGB値はリクエストの値をマッピング
            var color = _mapper.Map<ColorEntity>(request);

            color.Uuid = _uuidGenerator.GenerateUuid();
            color.CreatedAt = _dateTimeProvider.UtcNow;
            color.LastUpdatedAt = _dateTimeProvider.UtcNow;

            _context.Colors.Add(color);
            await _context.SaveChangesAsync();

            return _mapper.Map<ColorCreateDto>(color);
        }

        /// <summary>
        /// カラーテーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ColorDetailDto> GetColorByIdAsync(int id)
        {
            var color = await _context.Colors.FindAsync(id);

            if (color == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                // エンティティが存在しない場合は、例外を投げる
                throw new KeyNotFoundException(message);
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<ColorDetailDto>(color);
        }

        /// <summary>
        /// カラーテーブルを更新する
        /// </summary>
        /// <param name="request">更新用リクエストデータ</param>
        /// <returns></returns>
        public async Task<ColorUpdateDto> UpdateColorAsync(ColorUpdateRequest request)
        {
            var color = await _context.Colors.FindAsync(request.Id);

            if (color == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new KeyNotFoundException(message);
            }

            // カラー名、各RGB値はリクエストの値をマッピング
            color = _mapper.Map(request, color);

            color.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<ColorUpdateDto>(color);
        }

        /// <summary>
        /// カラーテーブルから削除する
        /// </summary>
        /// <param name="request">削除用リクエストデータ</param>
        /// <returns></returns>
        public async Task<ColorDeleteDto> DeleteColorAsync(int id)
        {
            var color = await _context.Colors.FindAsync(id);

            if (color == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new InvalidOperationException(message);
            }

            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();

            return new ColorDeleteDto
            {
                Id = color.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, color.Name)
            };
        }

        /// <summary>
        /// カラーテーブルを検索する
        /// </summary>
        /// <param name="request">検索用リクエストデータ</param>
        /// <returns></returns>
        public async Task<ColorSearchDto> SearchColorAsync(ColorSearchRequest request)
        {
            // colorsテーブルからクエリ可能なIQueryableを取得
            var query = _context.Colors.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchColorName))
            {
                switch (request.SearchMatchType)
                {
                    case SearchMatchType.EXACT:
                        query = query.Where(c => c.Name == request.SearchColorName);
                        break;
                    case SearchMatchType.PREFIX:
                        query = query.Where(c => c.Name.StartsWith(request.SearchColorName));
                        break;
                    case SearchMatchType.SUFFIX:
                        query = query.Where(c => c.Name.EndsWith(request.SearchColorName));
                        break;
                    case SearchMatchType.PARTIAL:
                        query = query.Where(c => c.Name.Contains(request.SearchColorName));
                        break;
                    case SearchMatchType.None:
                    default:
                        // 全件検索とする
                        break;
                }
            }

            var colors = await query.ToListAsync();

            return new ColorSearchDto
            {
                SearchItems = _mapper.Map<List<ColorListItemDto>>(colors),
                TotalCount = colors.Count
            };
        }
    }
}
