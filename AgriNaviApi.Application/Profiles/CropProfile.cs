using AgriNaviApi.Application.Requests.Crops;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.Crops;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 作付情報のマッピング
    /// </summary>
    public class CropProfile : Profile
    {
        public CropProfile()
        {
            #region Entity → Response
            CreateMap<ColorEntity, ColorResponse>();

            // 登録レスポンス: Entity→ CropCreateResponse
            CreateMap<CropEntity, CropCreateResponse>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

            // 詳細レスポンス：Entity→ CropDetailResponse
            CreateMap<CropEntity, CropDetailResponse>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

            // 更新レスポンス：Entity→ CropUpdateResponse
            CreateMap<CropEntity, CropUpdateResponse>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

            // 削除(手動でレスポンスを作成するため、マッピング不要)

            // 検索レスポンス：Entity→ CropListItemResponse
            CreateMap<CropEntity, CropListItemResponse>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));
            #endregion

            #region Request → Entity
            // 登録リクエスト： CropCreateRequest →  CropEntity
            CreateMap<CropCreateRequest, CropEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Group, opt => opt.Ignore())
                .ForMember(dest => dest.Color, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新リクエスト： CropUpdateRequest→ CropEntity
            CreateMap<CropUpdateRequest, CropEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Group, opt => opt.Ignore())
                .ForMember(dest => dest.Color, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
