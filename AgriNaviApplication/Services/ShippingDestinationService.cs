using AgriNaviApi.Application.DTOs.ShippingDestinations;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.ShippingDestinations;
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
    /// 出荷先名テーブルに関するサービス処理
    /// </summary>
    public class ShippingDestinationService : IShippingDestinationService
    {
        const string TableName = "出荷先名";

        private readonly AppDbContext _context;
        private readonly IUuidGenerator _uuidGenerator;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ShippingDestinationService(AppDbContext context, IUuidGenerator uuidGenerator, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _uuidGenerator = uuidGenerator;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// 出荷先名テーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DuplicateEntityException"></exception>
        public async Task<ShippingDestinationCreateDto> CreateShippingDestinationAsync(ShippingDestinationCreateRequest request)
        {
            // 出荷先名が既に登録されているかをチェック
            var exists = await _context.ShippingDestinations.AnyAsync(u => u.Name == request.Name && !u.IsDeleted);
            if (exists)
            {
                string message = string.Format(CommonErrorMessages.DuplicateErrorMessage, request.Name);
                throw new DuplicateEntityException(message);
            }

            // 出荷先名はリクエストの値をマッピング
            var shippingDestination = _mapper.Map<ShippingDestinationEntity>(request);

            shippingDestination.Uuid = _uuidGenerator.GenerateUuid();
            shippingDestination.CreatedAt = _dateTimeProvider.UtcNow;
            shippingDestination.LastUpdatedAt = _dateTimeProvider.UtcNow;

            _context.ShippingDestinations.Add(shippingDestination);
            await _context.SaveChangesAsync();

            return _mapper.Map<ShippingDestinationCreateDto>(shippingDestination);
        }

        /// <summary>
        /// 出荷先名テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ShippingDestinationDetailDto> GetShippingDestinationByIdAsync(int id)
        {
            var shippingDestination = await _context.ShippingDestinations.FindAsync(id);

            if (shippingDestination == null || shippingDestination.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                // エンティティが存在しない場合は、例外を投げる
                throw new KeyNotFoundException(message);
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<ShippingDestinationDetailDto>(shippingDestination);
        }

        /// <summary>
        /// 出荷先名テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ShippingDestinationUpdateDto> UpdateShippingDestinationAsync(ShippingDestinationUpdateRequest request)
        {
            var shippingDestination = await _context.ShippingDestinations.FindAsync(request.Id);

            if (shippingDestination == null || shippingDestination.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new KeyNotFoundException(message);
            }

            // 出荷先名はリクエストの値をマッピング
            shippingDestination = _mapper.Map(request, shippingDestination);

            shippingDestination.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<ShippingDestinationUpdateDto>(shippingDestination);
        }

        /// <summary>
        /// 出荷先名テーブルから削除する※削除フラグをtrueにするのみで、実際は削除しない
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ShippingDestinationDeleteDto> DeleteShippingDestinationAsync(int id)
        {
            var shippingDestination = await _context.ShippingDestinations.FindAsync(id);

            if (shippingDestination == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new InvalidOperationException(message);
            }

            if (shippingDestination.IsDeleted)
            {
                // 既に削除済みの場合
                string message = string.Format(CommonErrorMessages.DeletedMessage, TableName);
                throw new InvalidOperationException(message);
            }

            shippingDestination.IsDeleted = true;
            shippingDestination.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return new ShippingDestinationDeleteDto
            {
                Id = shippingDestination.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, shippingDestination.Name)
            };
        }

        /// <summary>
        /// 出荷先名テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ShippingDestinationSearchDto> SearchShippingDestinationAsync(ShippingDestinationSearchRequest request)
        {
            // shippingDestinationsテーブルからクエリ可能なIQueryableを取得
            var query = _context.ShippingDestinations.AsNoTracking().AsQueryable();

            // 削除していないデータが対象
            query = query.Where(c => !c.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.SearchShippingDestinationName))
            {
                switch (request.SearchMatchType)
                {
                    case SearchMatchType.EXACT:
                        query = query.Where(c => c.Name == request.SearchShippingDestinationName);
                        break;
                    case SearchMatchType.PREFIX:
                        query = query.Where(c => c.Name.StartsWith(request.SearchShippingDestinationName));
                        break;
                    case SearchMatchType.SUFFIX:
                        query = query.Where(c => c.Name.EndsWith(request.SearchShippingDestinationName));
                        break;
                    case SearchMatchType.PARTIAL:
                        query = query.Where(c => c.Name.Contains(request.SearchShippingDestinationName));
                        break;
                    case SearchMatchType.None:
                    default:
                        // 全件検索とする
                        break;
                }
            }

            var shippingDestinations = await query.ToListAsync();

            return new ShippingDestinationSearchDto
            {
                SearchItems = _mapper.Map<List<ShippingDestinationListItemDto>>(shippingDestinations),
                TotalCount = shippingDestinations.Count
            };
        }
    }
}
