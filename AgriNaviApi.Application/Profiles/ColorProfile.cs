using AgriNaviApi.Application.Requests.Colors;
using AgriNaviApi.Application.Responses.Colors;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// カラー情報のマッピング
    /// </summary>
    public class ColorProfile : Profile
    {
        public ColorProfile()
        {
            #region Entity → Response
            // 登録レスポンス: Entity→ColorCreateResponse
            CreateMap<ColorEntity, ColorCreateResponse>()
                .ForMember(dest => dest.RedValue, opt => opt.MapFrom(src => src.Red))
                .ForMember(dest => dest.GreenValue, opt => opt.MapFrom(src => src.Green))
                .ForMember(dest => dest.BlueValue, opt => opt.MapFrom(src => src.Blue));

            // 詳細レスポンス：Entity→ColorDetailResponse
            CreateMap<ColorEntity, ColorDetailResponse>()
                .ForMember(dest => dest.RedValue, opt => opt.MapFrom(src => src.Red))
                .ForMember(dest => dest.GreenValue, opt => opt.MapFrom(src => src.Green))
                .ForMember(dest => dest.BlueValue, opt => opt.MapFrom(src => src.Blue));

            // 更新レスポンス：Entity→ColorUpdateResponse
            CreateMap<ColorEntity, ColorUpdateResponse>()
                .ForMember(dest => dest.RedValue, opt => opt.MapFrom(src => src.Red))
                .ForMember(dest => dest.GreenValue, opt => opt.MapFrom(src => src.Green))
                .ForMember(dest => dest.BlueValue, opt => opt.MapFrom(src => src.Blue));

            // 削除(手動でレスポンスを作成するため、マッピング不要)

            // 検索レスポンス：Entity→ColorListItemResponse
            CreateMap<ColorEntity, ColorListItemResponse>()
                .ForMember(dest => dest.RedValue, opt => opt.MapFrom(src => src.Red))
                .ForMember(dest => dest.GreenValue, opt => opt.MapFrom(src => src.Green))
                .ForMember(dest => dest.BlueValue, opt => opt.MapFrom(src => src.Blue));
            #endregion

            #region Request → Entity
            // 登録リクエスト：ColorCreateRequest → ColorEntity
            CreateMap<ColorCreateRequest, ColorEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Red, opt => opt.MapFrom(src => src.RedValue))
                .ForMember(dest => dest.Green, opt => opt.MapFrom(src => src.GreenValue))
                .ForMember(dest => dest.Blue, opt => opt.MapFrom(src => src.BlueValue));

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新リクエスト：ColorUpdateRequest→ColorEntity
            CreateMap<ColorUpdateRequest, ColorEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Red, opt => opt.MapFrom(src => src.RedValue))
                .ForMember(dest => dest.Green, opt => opt.MapFrom(src => src.GreenValue))
                .ForMember(dest => dest.Blue, opt => opt.MapFrom(src => src.BlueValue));

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
