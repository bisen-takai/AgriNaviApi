using AgriNaviApi.Application.DTOs.ShipmentRecordDetails;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.ShipmentRecordDetails;
using AgriNaviApi.Common.Interfaces;
using AgriNaviApi.Common.Resources;
using AgriNaviApi.Infrastructure.Persistence.Contexts;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace AgriNaviApi.Application.Services
{
    /// <summary>
    /// 出荷記録詳細テーブルに関するサービス処理
    /// </summary>
    public class ShipmentRecordDetailService : IShipmentRecordDetailService
    {
        const string TableName = "出荷記録詳細";

        private readonly AppDbContext _context;
        private readonly IUuidGenerator _uuidGenerator;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ShipmentRecordDetailService(AppDbContext context, IUuidGenerator uuidGenerator, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _uuidGenerator = uuidGenerator;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// 出荷記録詳細テーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DuplicateEntityException"></exception>
        public async Task<ShipmentRecordDetailCreateDto> CreateShipmentRecordDetailAsync(ShipmentRecordDetailCreateRequest request)
        {
            // 出荷記録詳細はリクエストの値をマッピング
            var shipmentRecordDetail = _mapper.Map<ShipmentRecordDetailEntity>(request);

            shipmentRecordDetail.Uuid = _uuidGenerator.GenerateUuid();
            shipmentRecordDetail.CreatedAt = _dateTimeProvider.UtcNow;
            shipmentRecordDetail.LastUpdatedAt = _dateTimeProvider.UtcNow;

            _context.ShipmentRecordDetails.Add(shipmentRecordDetail);
            await _context.SaveChangesAsync();

            return _mapper.Map<ShipmentRecordDetailCreateDto>(shipmentRecordDetail);
        }

        /// <summary>
        /// 出荷記録詳細テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ShipmentRecordDetailDetailDto> GetShipmentRecordDetailByIdAsync(int id)
        {
            var shipmentRecordDetail = await _context.ShipmentRecordDetails.FindAsync(id);

            if (shipmentRecordDetail == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                // エンティティが存在しない場合は、例外を投げる
                throw new KeyNotFoundException(message);
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<ShipmentRecordDetailDetailDto>(shipmentRecordDetail);
        }

        /// <summary>
        /// 出荷記録詳細テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ShipmentRecordDetailUpdateDto> UpdateShipmentRecordDetailAsync(ShipmentRecordDetailUpdateRequest request)
        {
            var shipmentRecordDetail = await _context.ShipmentRecordDetails.FindAsync(request.Id);

            if (shipmentRecordDetail == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new KeyNotFoundException(message);
            }

            // 出荷記録詳細はリクエストの値をマッピング
            shipmentRecordDetail = _mapper.Map(request, shipmentRecordDetail);

            shipmentRecordDetail.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<ShipmentRecordDetailUpdateDto>(shipmentRecordDetail);
        }

        /// <summary>
        /// 出荷記録詳細テーブルから削除する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ShipmentRecordDetailDeleteDto> DeleteShipmentRecordDetailAsync(int id)
        {
            var shipmentRecordDetail = await _context.ShipmentRecordDetails.FindAsync(id);

            if (shipmentRecordDetail == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new InvalidOperationException(message);
            }

            _context.ShipmentRecordDetails.Remove(shipmentRecordDetail);
            await _context.SaveChangesAsync();

            return new ShipmentRecordDetailDeleteDto
            {
                Id = shipmentRecordDetail.Id,
                ShipmentRecordId = shipmentRecordDetail.ShipmentRecordId,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, TableName)
            };
        }

        /// <summary>
        /// 出荷記録詳細テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ShipmentRecordDetailSearchDto> SearchShipmentRecordDetailAsync(ShipmentRecordDetailSearchRequest request)
        {
            // shipmentRecordDetailsテーブルからクエリ可能なIQueryableを取得
            var query = _context.ShipmentRecordDetails.AsNoTracking().AsQueryable();

            // 出荷記録IDと同じ出荷記録詳細を取得
            query = query.Where(c => c.ShipmentRecordId <= request.ShipmentRecordId);

            var shipmentRecordDetails = await query.ToListAsync();

            return new ShipmentRecordDetailSearchDto
            {
                SearchItems = _mapper.Map<List<ShipmentRecordDetailListItemDto>>(shipmentRecordDetails),
                TotalCount = shipmentRecordDetails.Count
            };
        }
    }
}
