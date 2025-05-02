using AgriNaviApi.Application.DTOs.ShipmentRecordDetails;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AgriNaviApi.Application.Services
{
    /// <summary>
    /// 出荷記録テーブルに関するサービス処理
    /// </summary>
    public class ShipmentRecordWithDetailService : IShipmentRecordWithDetailService
    {
        const string TableName = "出荷記録";

        private readonly AppDbContext _context;
        private readonly IUuidGenerator _uuidGenerator;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ShipmentRecordWithDetailService(AppDbContext context, IUuidGenerator uuidGenerator, IMapper mapper, IDateTimeProvider dateTimeProvider)
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
        public async Task<ShipmentRecordWithDetailCreateDto> CreateShipmentRecordAsync(ShipmentRecordWithDetailCreateRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                bool exists = await _context.ShipmentRecords.AnyAsync(r =>
                    r.RecordDate == request.RecordDate &&
                    r.FieldId == request.FieldId &&
                    r.CropId == request.CropId &&
                    r.SeasonCropScheduleId == request.SeasonCropScheduleId &&
                    !r.IsDeleted
                    );

                if (exists)
                {
                    await transaction.RollbackAsync();
                    string message = string.Format(CommonErrorMessages.DuplicateErrorMessage, TableName);
                    throw new DuplicateEntityException(message);
                }

                // 出荷記録名はリクエストの値をマッピング
                var shipmentRecord = _mapper.Map<ShipmentRecordEntity>(request);

                shipmentRecord.Uuid = _uuidGenerator.GenerateUuid();
                shipmentRecord.CreatedAt = _dateTimeProvider.UtcNow;
                shipmentRecord.LastUpdatedAt = _dateTimeProvider.UtcNow;

                _context.ShipmentRecords.Add(shipmentRecord);
                await _context.SaveChangesAsync();

                var detailRecords = new List<ShipmentRecordDetailEntity>();

                foreach (var detailRequest in request.Details)
                {
                    var detailRecord = _mapper.Map<ShipmentRecordDetailEntity>(detailRequest);

                    detailRecord.ShipmentRecordId = shipmentRecord.Id;
                    detailRecord.Uuid = _uuidGenerator.GenerateUuid();
                    detailRecord.CreatedAt = _dateTimeProvider.UtcNow;
                    detailRecord.LastUpdatedAt = _dateTimeProvider.UtcNow;

                    detailRecords.Add(detailRecord);
                    _context.ShipmentRecordDetails.Add(detailRecord);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var resonse = _mapper.Map<ShipmentRecordWithDetailCreateDto>(shipmentRecord);
                resonse.Details = _mapper.Map<List<ShipmentRecordDetailCreateDto>>(detailRecords);

                return resonse;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// 出荷記録テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ShipmentRecordWithDetailDetailDto> GetShipmentRecordByIdAsync(int id)
        {
            var shipmentRecord = await _context.ShipmentRecords
                .Include(r => r.Details)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

            if (shipmentRecord == null)
            {
                string message = string.Format(CommonErrorMessages.NotFoundMessage, TableName);
                // エンティティが存在しない場合は、例外を投げる
                throw new KeyNotFoundException(message);
            }

            // エンティティから DTO へ変換する
            var response = _mapper.Map<ShipmentRecordWithDetailDetailDto>(shipmentRecord);
            response.Details = _mapper.Map<List<ShipmentRecordDetailDetailDto>>(shipmentRecord.Details);
            return response;
        }

        /// <summary>
        /// 出荷記録テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ShipmentRecordWithDetailUpdateDto> UpdateShipmentRecordAsync(ShipmentRecordWithDetailUpdateRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
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

                _context.ShipmentRecords.Add(shipmentRecord);
                await _context.SaveChangesAsync();

                foreach (var existingDetail in shipmentRecord.Details.ToList())
                {
                    var match = request.Details.FirstOrDefault(d => d.Id == existingDetail.Id);

                    if (match != null)
                    {
                        _mapper.Map(match, existingDetail);
                        existingDetail.LastUpdatedAt = _dateTimeProvider.UtcNow;
                    }
                    else
                    {
                        _context.ShipmentRecordDetails.Remove(existingDetail);
                    }
                }

                var newDetails = request.Details.Where(d => d.Id == 0);

                foreach (var newDetail in newDetails)
                {
                    var newEntity = _mapper.Map<ShipmentRecordDetailEntity>(newDetail);
                    newEntity.ShipmentRecordId = shipmentRecord.Id; // 親IDの紐づけ
                    newEntity.Uuid = _uuidGenerator.GenerateUuid();
                    newEntity.CreatedAt = _dateTimeProvider.UtcNow;
                    newEntity.LastUpdatedAt = _dateTimeProvider.UtcNow;

                    shipmentRecord.Details.Add(newEntity);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // エンティティから DTO へ変換する
                var response = _mapper.Map<ShipmentRecordWithDetailUpdateDto>(shipmentRecord);
                response.Details = _mapper.Map<List<ShipmentRecordDetailUpdateDto>>(shipmentRecord.Details);
                return response;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// 出荷記録テーブルから削除する※削除フラグをtrueにするのみで、実際は削除しない
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ShipmentRecordWithDetailDeleteDto> DeleteShipmentRecordAsync(int id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var shipmentRecord = await _context.ShipmentRecords
                    .Include(r => r.Details)
                    .FirstOrDefaultAsync(r => r.Id == id);

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

                foreach (var detail in shipmentRecord.Details.ToList())
                {
                    _context.ShipmentRecordDetails.Remove(detail);
                }

                shipmentRecord.IsDeleted = true;
                shipmentRecord.LastUpdatedAt = _dateTimeProvider.UtcNow;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ShipmentRecordWithDetailDeleteDto
                {
                    Id = shipmentRecord.Id,
                    IsDeleted = true,
                    Message = string.Format(CommonMessages.DeleteMessage, TableName)
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// 出荷記録テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ShipmentRecordWithDetailSearchDto> SearchShipmentRecordAsync(ShipmentRecordWithDetailSearchRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // 削除していないデータが対象
            var shipmentRecord = _context.ShipmentRecords
                .AsNoTracking()
                .Include(r => r.Details)
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

            return new ShipmentRecordWithDetailSearchDto
            {
                SearchItems = _mapper.Map<List<ShipmentRecordWithDetailListItemDto>>(shipmentRecords),
                TotalCount = shipmentRecords.Count
            };
        }
    }
}
