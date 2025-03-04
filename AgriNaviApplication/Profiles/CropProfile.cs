using AgriNaviApi.Application.DTOs.Crops;
using AgriNaviApi.Application.Requests.Crops;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 作付名テーブルに関するマッピング管理
    /// </summary>
    public class CropProfile : Profile
    {
        public CropProfile()
        {
            #region Entity → Dto
            CreateMap<CropEntity, CropCreateDto>()
                .ForMember(dto => dto.GroupName, opt => opt.MapFrom(entity => entity.Group.Name))
                .ForMember(dto => dto.ColorName, opt => opt.MapFrom(entity => entity.Color.Name))
                .ForMember(dto => dto.ColorRedValue, opt => opt.MapFrom(entity => entity.Color.RedValue))
                .ForMember(dto => dto.ColorGreenValue, opt => opt.MapFrom(entity => entity.Color.GreenValue))
                .ForMember(dto => dto.ColorBlueValue, opt => opt.MapFrom(entity => entity.Color.BlueValue));

            CreateMap<CropEntity, CropUpdateDto>()
                .ForMember(dto => dto.GroupName, opt => opt.MapFrom(entity => entity.Group.Name))
                .ForMember(dto => dto.ColorName, opt => opt.MapFrom(entity => entity.Color.Name))
                .ForMember(dto => dto.ColorRedValue, opt => opt.MapFrom(entity => entity.Color.RedValue))
                .ForMember(dto => dto.ColorGreenValue, opt => opt.MapFrom(entity => entity.Color.GreenValue))
                .ForMember(dto => dto.ColorBlueValue, opt => opt.MapFrom(entity => entity.Color.BlueValue));

            CreateMap<CropEntity, CropDeleteDto>();

            CreateMap<CropEntity, CropDetailDto>()
                .ForMember(dto => dto.GroupName, opt => opt.MapFrom(entity => entity.Group.Name))
                .ForMember(dto => dto.ColorName, opt => opt.MapFrom(entity => entity.Color.Name))
                .ForMember(dto => dto.ColorRedValue, opt => opt.MapFrom(entity => entity.Color.RedValue))
                .ForMember(dto => dto.ColorGreenValue, opt => opt.MapFrom(entity => entity.Color.GreenValue))
                .ForMember(dto => dto.ColorBlueValue, opt => opt.MapFrom(entity => entity.Color.BlueValue));

            // 検索結果用
            CreateMap<CropEntity, CropListItemDto>()
                .ForMember(dto => dto.GroupName, opt => opt.MapFrom(entity => entity.Group.Name))
                .ForMember(dto => dto.ColorName, opt => opt.MapFrom(entity => entity.Color.Name))
                .ForMember(dto => dto.ColorRedValue, opt => opt.MapFrom(entity => entity.Color.RedValue))
                .ForMember(dto => dto.ColorGreenValue, opt => opt.MapFrom(entity => entity.Color.GreenValue))
                .ForMember(dto => dto.ColorBlueValue, opt => opt.MapFrom(entity => entity.Color.BlueValue));
            #endregion

            #region Request → Entity
            // 新規作成(Id, Uuid, CreatedAt, LastUpdatedAt は無視)
            CreateMap<CropCreateRequest, CropEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新(Id,UUIDは変更不可, LastUpdatedAtはサービス層で設定)
            CreateMap<CropUpdateRequest, CropEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
