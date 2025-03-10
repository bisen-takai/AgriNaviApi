using AgriNaviApi.Application.DTOs.Crops;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.Crops;
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
    /// 作付テーブルに関するサービス処理
    /// </summary>
    public class CropService : ICropService
    {
        const string TableName = "作付";

        private readonly AppDbContext _context;
        private readonly IUuidGenerator _uuidGenerator;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public CropService(AppDbContext context, IUuidGenerator uuidGenerator, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _uuidGenerator = uuidGenerator;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// 作付テーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DuplicateEntityException"></exception>
        public async Task<CropCreateDto> CreateCropAsync(CropCreateRequest request)
        {
            // 作付名が既に登録されているかをチェック
            var exists = await _context.Crops.AnyAsync(u => u.Name == request.Name && !u.IsDeleted);
            if (exists)
            {
                string message = string.Format(CommonErrorMessages.DuplicateErrorMessage, request.Name);
                throw new DuplicateEntityException(message);
            }

            // 作付名はリクエストの値をマッピング
            var crop = _mapper.Map<CropEntity>(request);

            crop.Uuid = _uuidGenerator.GenerateUuid();
            crop.CreatedAt = _dateTimeProvider.UtcNow;
            crop.LastUpdatedAt = _dateTimeProvider.UtcNow;

            _context.Crops.Add(crop);
            await _context.SaveChangesAsync();

            return _mapper.Map<CropCreateDto>(crop);
        }

        /// <summary>
        /// 作付テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<CropDetailDto> GetCropByIdAsync(int id)
        {
            var crop = await _context.Crops.FindAsync(id);

            if (crop == null || crop.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                // エンティティが存在しない場合は、例外を投げる
                throw new KeyNotFoundException(message);
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<CropDetailDto>(crop);
        }

        /// <summary>
        /// 作付テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<CropUpdateDto> UpdateCropAsync(CropUpdateRequest request)
        {
            var crop = await _context.Crops.FindAsync(request.Id);

            if (crop == null || crop.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new KeyNotFoundException(message);
            }

            // 作付名はリクエストの値をマッピング
            crop = _mapper.Map(request, crop);

            crop.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<CropUpdateDto>(crop);
        }

        /// <summary>
        /// 作付テーブルから削除する※削除フラグをtrueにするのみで、実際は削除しない
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<CropDeleteDto> DeleteCropAsync(CropDeleteRequest request)
        {
            var crop = await _context.Crops.FindAsync(request.Id);

            if (crop == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new InvalidOperationException(message);
            }

            if (crop.IsDeleted)
            {
                // 既に削除済みの場合
                string message = string.Format(CommonErrorMessages.DeletedMessage, TableName);
                throw new InvalidOperationException(message);
            }

            crop.IsDeleted = true;
            crop.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return new CropDeleteDto
            {
                Id = crop.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, crop.Name)
            };
        }

        /// <summary>
        /// 作付テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CropSearchDto> SearchCropAsync(CropSearchRequest request)
        {
            // cropsテーブルからクエリ可能なIQueryableを取得
            var query = _context.Crops.AsNoTracking().AsQueryable();

            // 削除していないデータが対象
            query = query.Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchCropName))
            {
                switch (request.SearchMatchType)
                {
                    case SearchMatchType.EXACT:
                        query = query.Where(c => c.Name == request.SearchCropName);
                        break;
                    case SearchMatchType.PREFIX:
                        query = query.Where(c => c.Name.StartsWith(request.SearchCropName));
                        break;
                    case SearchMatchType.SUFFIX:
                        query = query.Where(c => c.Name.EndsWith(request.SearchCropName));
                        break;
                    case SearchMatchType.PARTIAL:
                        query = query.Where(c => c.Name.Contains(request.SearchCropName));
                        break;
                    case SearchMatchType.None:
                    default:
                        // 全件検索とする
                        break;
                }
            }

            var crops = await query.ToListAsync();

            return new CropSearchDto
            {
                SearchItems = _mapper.Map<List<CropListItemDto>>(crops),
                TotalCount = crops.Count
            };
        }
    }
}
