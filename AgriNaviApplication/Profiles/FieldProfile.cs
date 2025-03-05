using AgriNaviApi.Application.DTOs.Fields;
using AgriNaviApi.Application.Requests.Fields;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 圃場テーブルに関するマッピング管理
    /// </summary>
    public class FieldProfile : Profile
    {
        public FieldProfile()
        {
            #region Entity → Dto
            CreateMap<FieldEntity, FieldCreateDto>()
                .ForMember(dto => dto.GroupName, opt => opt.MapFrom(entity => entity.Group.Name))
                .ForMember(dto => dto.ColorName, opt => opt.MapFrom(entity => entity.Color.Name))
                .ForMember(dto => dto.ColorRedValue, opt => opt.MapFrom(entity => entity.Color.RedValue))
                .ForMember(dto => dto.ColorGreenValue, opt => opt.MapFrom(entity => entity.Color.GreenValue))
                .ForMember(dto => dto.ColorBlueValue, opt => opt.MapFrom(entity => entity.Color.BlueValue));

            CreateMap<FieldEntity, FieldUpdateDto>()
                .ForMember(dto => dto.GroupName, opt => opt.MapFrom(entity => entity.Group.Name))
                .ForMember(dto => dto.ColorName, opt => opt.MapFrom(entity => entity.Color.Name))
                .ForMember(dto => dto.ColorRedValue, opt => opt.MapFrom(entity => entity.Color.RedValue))
                .ForMember(dto => dto.ColorGreenValue, opt => opt.MapFrom(entity => entity.Color.GreenValue))
                .ForMember(dto => dto.ColorBlueValue, opt => opt.MapFrom(entity => entity.Color.BlueValue));

            CreateMap<FieldEntity, FieldDeleteDto>();

            CreateMap<FieldEntity, FieldDetailDto>()
                .ForMember(dto => dto.GroupName, opt => opt.MapFrom(entity => entity.Group.Name))
                .ForMember(dto => dto.ColorName, opt => opt.MapFrom(entity => entity.Color.Name))
                .ForMember(dto => dto.ColorRedValue, opt => opt.MapFrom(entity => entity.Color.RedValue))
                .ForMember(dto => dto.ColorGreenValue, opt => opt.MapFrom(entity => entity.Color.GreenValue))
                .ForMember(dto => dto.ColorBlueValue, opt => opt.MapFrom(entity => entity.Color.BlueValue));

            // 検索結果用
            CreateMap<FieldEntity, FieldListItemDto>()
                .ForMember(dto => dto.GroupName, opt => opt.MapFrom(entity => entity.Group.Name))
                .ForMember(dto => dto.ColorName, opt => opt.MapFrom(entity => entity.Color.Name))
                .ForMember(dto => dto.ColorRedValue, opt => opt.MapFrom(entity => entity.Color.RedValue))
                .ForMember(dto => dto.ColorGreenValue, opt => opt.MapFrom(entity => entity.Color.GreenValue))
                .ForMember(dto => dto.ColorBlueValue, opt => opt.MapFrom(entity => entity.Color.BlueValue));
            #endregion

            #region Request → Entity
            // 新規作成(Id, Uuid, CreatedAt, LastUpdatedAt は無視)
            CreateMap<FieldCreateRequest, FieldEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新(Id,UUIDは変更不可, LastUpdatedAtはサービス層で設定)
            CreateMap<FieldUpdateRequest, FieldEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
