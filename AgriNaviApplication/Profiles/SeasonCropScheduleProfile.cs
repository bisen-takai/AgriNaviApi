using AgriNaviApi.Application.DTOs.SeasonCropSchedules;
using AgriNaviApi.Application.Requests.SeasonCropSchedules;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 作付計画テーブルに関するマッピング管理
    /// </summary>
    public class SeasonCropScheduleProfile : Profile
    {
        public SeasonCropScheduleProfile()
        {
            #region Entity → Dto
            CreateMap<SeasonCropScheduleEntity, SeasonCropScheduleCreateDto>()
                .ForMember(dto => dto.CropName, opt => opt.MapFrom(entity => entity.Crop.Name));

            CreateMap<SeasonCropScheduleEntity, SeasonCropScheduleUpdateDto>()
                .ForMember(dto => dto.CropName, opt => opt.MapFrom(entity => entity.Crop.Name));

            CreateMap<SeasonCropScheduleEntity, SeasonCropScheduleDeleteDto>();

            CreateMap<SeasonCropScheduleEntity, SeasonCropScheduleDetailDto>()
                .ForMember(dto => dto.CropName, opt => opt.MapFrom(entity => entity.Crop.Name));

            // 検索結果用
            CreateMap<SeasonCropScheduleEntity, SeasonCropScheduleListItemDto>()
                .ForMember(dto => dto.CropName, opt => opt.MapFrom(entity => entity.Crop.Name));
            #endregion

            #region Request → Entity
            // 新規作成(Id, Uuid, CreatedAt, LastUpdatedAt は無視)
            CreateMap<SeasonCropScheduleCreateRequest, SeasonCropScheduleEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新(Id,UUIDは変更不可, LastUpdatedAtはサービス層で設定)
            CreateMap<SeasonCropScheduleUpdateRequest, SeasonCropScheduleEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
