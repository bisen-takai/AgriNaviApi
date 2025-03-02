using AgriNaviApi.Application.DTOs.Colors;
using AgriNaviApi.Application.Requests.Colors;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// カラーテーブルに関するマッピング管理
    /// </summary>
    public class ColorProfile : Profile
    {
        public ColorProfile()
        {
            #region Entity → Dto
            CreateMap<ColorEntity, ColorCreateDto>();

            CreateMap<ColorEntity, ColorUpdateDto>();

            CreateMap<ColorEntity, ColorDeleteDto>();

            CreateMap<ColorEntity, ColorDetailDto>();

            // 検索結果用
            CreateMap<ColorEntity, ColorListItemDto>();

            #endregion

            #region Request → Entity
            // 新規作成(Id, Uuid, CreatedAt, LastUpdatedAt は無視)
            CreateMap<ColorCreateRequest, ColorEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新(Id,UUIDは変更不可, LastUpdatedAtはサービス層で設定)
            CreateMap<ColorUpdateRequest, ColorEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
