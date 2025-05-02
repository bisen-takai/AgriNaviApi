using AgriNaviApi.Application.DTOs.ShipmentRecords;
using AgriNaviApi.Application.Requests.ShipmentRecords;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 出荷記録テーブルに関するマッピング管理
    /// </summary>
    public class ShipmentRecordWithDetailProfile : Profile
    {
        public ShipmentRecordWithDetailProfile()
        {
            #region Entity → Dto
            CreateMap<ShipmentRecordEntity, ShipmentRecordWithDetailCreateDto>()
                .ForMember(dto => dto.FieldName, opt => opt.MapFrom(entity => entity.Field.Name))
                .ForMember(dto => dto.CropName, opt => opt.MapFrom(entity => entity.Crop.Name))
                .ForMember(dto => dto.SeasonCropScheduleName, opt => opt.MapFrom(entity => entity.SeasonCropSchedule.Name));

            CreateMap<ShipmentRecordEntity, ShipmentRecordWithDetailUpdateDto>()
                .ForMember(dto => dto.FieldName, opt => opt.MapFrom(entity => entity.Field.Name))
                .ForMember(dto => dto.CropName, opt => opt.MapFrom(entity => entity.Crop.Name))
                .ForMember(dto => dto.SeasonCropScheduleName, opt => opt.MapFrom(entity => entity.SeasonCropSchedule.Name));

            CreateMap<ShipmentRecordEntity, ShipmentRecordWithDetailDeleteDto>();

            CreateMap<ShipmentRecordEntity, ShipmentRecordWithDetailDetailDto>()
                .ForMember(dto => dto.FieldName, opt => opt.MapFrom(entity => entity.Field.Name))
                .ForMember(dto => dto.CropName, opt => opt.MapFrom(entity => entity.Crop.Name))
                .ForMember(dto => dto.SeasonCropScheduleName, opt => opt.MapFrom(entity => entity.SeasonCropSchedule.Name));

            // 検索結果用
            CreateMap<ShipmentRecordEntity, ShipmentRecordWithDetailListItemDto>()
                .ForMember(dto => dto.FieldName, opt => opt.MapFrom(entity => entity.Field.Name))
                .ForMember(dto => dto.CropName, opt => opt.MapFrom(entity => entity.Crop.Name))
                .ForMember(dto => dto.SeasonCropScheduleName, opt => opt.MapFrom(entity => entity.SeasonCropSchedule.Name));
            #endregion

            #region Request → Entity
            // 新規作成(Id, Uuid, CreatedAt, LastUpdatedAt は無視)
            CreateMap<ShipmentRecordWithDetailCreateRequest, ShipmentRecordEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新(Id,UUIDは変更不可, LastUpdatedAtはサービス層で設定)
            CreateMap<ShipmentRecordWithDetailUpdateRequest, ShipmentRecordEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
