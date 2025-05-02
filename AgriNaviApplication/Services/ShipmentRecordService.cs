using AgriNaviApi.Application.DTOs.ShipmentRecords;
using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Requests.ShipmentRecords;
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
    /// 出荷記録テーブルに関するサービス処理
    /// </summary>
    public class ShipmentRecordService : IShipmentRecordService
    {
        const string TableName = "出荷記録";

        private readonly AppDbContext _context;
        private readonly IUuidGenerator _uuidGenerator;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ShipmentRecordService(AppDbContext context, IUuidGenerator uuidGenerator, IMapper mapper, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _uuidGenerator = uuidGenerator;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// 出荷記録テーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DuplicateEntityException"></exception>
        public async Task<ShipmentRecordCreateDto> CreateShipmentRecordAsync(ShipmentRecordCreateRequest request)
        {
            // 出荷記録名はリクエストの値をマッピング
            var shipmentRecord = _mapper.Map<ShipmentRecordEntity>(request);

            shipmentRecord.Uuid = _uuidGenerator.GenerateUuid();
            shipmentRecord.CreatedAt = _dateTimeProvider.UtcNow;
            shipmentRecord.LastUpdatedAt = _dateTimeProvider.UtcNow;

            _context.ShipmentRecords.Add(shipmentRecord);
            await _context.SaveChangesAsync();

            return _mapper.Map<ShipmentRecordCreateDto>(shipmentRecord);
        }

        /// <summary>
        /// 出荷記録テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ShipmentRecordDetailDto> GetShipmentRecordByIdAsync(int id)
        {
            var shipmentRecord = await _context.ShipmentRecords.FindAsync(id);

            if (shipmentRecord == null || shipmentRecord.IsDeleted)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                // エンティティが存在しない場合は、例外を投げる
                throw new KeyNotFoundException(message);
            }

            // エンティティから DTO へ変換する
            return _mapper.Map<ShipmentRecordDetailDto>(shipmentRecord);
        }

        /// <summary>
        /// 出荷記録テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ShipmentRecordUpdateDto> UpdateShipmentRecordAsync(ShipmentRecordUpdateRequest request)
        {
            var shipmentRecord = await _context.ShipmentRecords
                .Include(r => r.Details)
                .FirstOrDefaultAsync(r => r.Id == request.Id && !r.IsDeleted);

            if (shipmentRecord == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new KeyNotFoundException(message);
            }

            // 出荷記録名はリクエストの値をマッピング
            shipmentRecord = _mapper.Map(request, shipmentRecord);

            shipmentRecord.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return _mapper.Map<ShipmentRecordUpdateDto>(shipmentRecord);
        }

        /// <summary>
        /// 出荷記録テーブルから削除する※削除フラグをtrueにするのみで、実際は削除しない
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ShipmentRecordDeleteDto> DeleteShipmentRecordAsync(int id)
        {
            var shipmentRecord = await _context.ShipmentRecords.FindAsync(id);

            if (shipmentRecord == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                throw new InvalidOperationException(message);
            }

            if (shipmentRecord.IsDeleted)
            {
                // 既に削除済みの場合
                string message = string.Format(CommonErrorMessages.DeletedMessage, TableName);
                throw new AlreadyDeletedException(message);
            }

            shipmentRecord.IsDeleted = true;
            shipmentRecord.LastUpdatedAt = _dateTimeProvider.UtcNow;

            await _context.SaveChangesAsync();

            return new ShipmentRecordDeleteDto
            {
                Id = shipmentRecord.Id,
                IsDeleted = true,
                Message = string.Format(CommonMessages.DeleteMessage, TableName)
            };
        }

        /// <summary>
        /// 出荷記録テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ShipmentRecordSearchDto> SearchShipmentRecordAsync(ShipmentRecordSearchRequest request)
        {
            // shipmentRecordsテーブルからクエリ可能なIQueryableを取得
            var shipmentRecord = _context.ShipmentRecords
                .AsNoTracking()
                .Where(r => !r.IsDeleted);

            if (request.SeasonCropScheduleId > 0)
            {
                shipmentRecord = shipmentRecord.Where(r => r.SeasonCropScheduleId == request.SeasonCropScheduleId);
            }
            else
            {
                if (request.StartDate != null)
                {
                    // request.StartDate 以降のデータを取得
                    shipmentRecord = shipmentRecord.Where(c => c.RecordDate >= request.StartDate);
                }

                if (request.EndDate != null)
                {
                    // request.EndDate 以下のデータを取得
                    shipmentRecord = shipmentRecord.Where(c => c.RecordDate <= request.EndDate);
                }
            }

            var shipmentRecords = await shipmentRecord.ToListAsync();

            return new ShipmentRecordSearchDto
            {
                SearchItems = _mapper.Map<List<ShipmentRecordListItemDto>>(shipmentRecords),
                TotalCount = shipmentRecords.Count
            };
        }
    }
}
