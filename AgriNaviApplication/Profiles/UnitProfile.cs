using AgriNaviApi.Application.DTOs.Units;
using AgriNaviApi.Application.Requests.Units;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// カラーテーブルに関するマッピング管理
    /// </summary>
    public class UnitProfile : Profile
    {
        public UnitProfile()
        {
            #region Entity → Dto
            CreateMap<UnitEntity, UnitCreateDto>();

            CreateMap<UnitEntity, UnitUpdateDto>();

            CreateMap<UnitEntity, UnitDeleteDto>();

            CreateMap<UnitEntity, UnitDetailDto>();
            #endregion

            #region Request → Entity
            // 新規作成(Id, Uuid, CreatedAt, LastUpdatedAt は無視)
            CreateMap<UnitCreateRequest, UnitEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新(Id,UUIDは変更不可, LastUpdatedAtはサービス層で設定)
            CreateMap<UnitUpdateRequest, UnitEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)
            #endregion
        }
    }
}
