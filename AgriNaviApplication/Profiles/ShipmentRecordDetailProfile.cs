using AgriNaviApi.Application.DTOs.ShipmentRecordDetails;
using AgriNaviApi.Application.Requests.ShipmentRecordDetails;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 出荷記録詳細テーブルに関するマッピング管理
    /// </summary>
    public class ShipmentRecordDetailProfile : Profile
    {
        public ShipmentRecordDetailProfile()
        {
            #region Entity → Dto
            CreateMap<ShipmentRecordDetailEntity, ShipmentRecordDetailCreateDto>()
                .ForMember(dto => dto.ShippingDestinationName, opt => opt.MapFrom(entity => entity.ShippingDestination.Name))
                .ForMember(dto => dto.QualityStandardName, opt => opt.MapFrom(entity => entity.QualityStandard.Name))
                .ForMember(dto => dto.UnitName, opt => opt.MapFrom(entity => entity.Unit.Name));

            CreateMap<ShipmentRecordDetailEntity, ShipmentRecordDetailUpdateDto>()
                .ForMember(dto => dto.ShippingDestinationName, opt => opt.MapFrom(entity => entity.ShippingDestination.Name))
                .ForMember(dto => dto.QualityStandardName, opt => opt.MapFrom(entity => entity.QualityStandard.Name))
                .ForMember(dto => dto.UnitName, opt => opt.MapFrom(entity => entity.Unit.Name));

            CreateMap<ShipmentRecordDetailEntity, ShipmentRecordDetailDeleteDto>();

            CreateMap<ShipmentRecordDetailEntity, ShipmentRecordDetailDetailDto>()
                .ForMember(dto => dto.ShippingDestinationName, opt => opt.MapFrom(entity => entity.ShippingDestination.Name))
                .ForMember(dto => dto.QualityStandardName, opt => opt.MapFrom(entity => entity.QualityStandard.Name))
                .ForMember(dto => dto.UnitName, opt => opt.MapFrom(entity => entity.Unit.Name));

            // 検索結果用
            CreateMap<ShipmentRecordDetailEntity, ShipmentRecordDetailListItemDto>()
                .ForMember(dto => dto.ShippingDestinationName, opt => opt.MapFrom(entity => entity.ShippingDestination.Name))
                .ForMember(dto => dto.QualityStandardName, opt => opt.MapFrom(entity => entity.QualityStandard.Name))
                .ForMember(dto => dto.UnitName, opt => opt.MapFrom(entity => entity.Unit.Name));
            #endregion

            #region Request → Entity
            // 新規作成(Id, Uuid, CreatedAt, LastUpdatedAt は無視)
            CreateMap<ShipmentRecordDetailCreateRequest, ShipmentRecordDetailEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新(Id,UUIDは変更不可, LastUpdatedAtはサービス層で設定)
            CreateMap<ShipmentRecordDetailUpdateRequest, ShipmentRecordDetailEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
