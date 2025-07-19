using AgriNaviApi.Application.Requests.Fields;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.Fields;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 圃場情報のマッピング
    /// </summary>
    public class FieldProfile : Profile
    {
        public FieldProfile()
        {
            #region Entity → Response
            CreateMap<ColorEntity, ColorResponse>();

            // 登録レスポンス: Entity→ FieldCreateResponse
            CreateMap<FieldEntity, FieldCreateResponse>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

            // 詳細レスポンス：Entity→ FieldDetailResponse
            CreateMap<FieldEntity, FieldDetailResponse>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

            // 更新レスポンス：Entity→ FieldUpdateResponse
            CreateMap<FieldEntity, FieldUpdateResponse>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

            // 削除(手動でレスポンスを作成するため、マッピング不要)

            // 検索レスポンス：Entity→ FieldListItemResponse
            CreateMap<FieldEntity, FieldListItemResponse>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));
            #endregion

            #region Request → Entity
            // 登録リクエスト： FieldCreateRequest →  FieldEntity
            CreateMap<FieldCreateRequest, FieldEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Group, opt => opt.Ignore())
                .ForMember(dest => dest.Color, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新リクエスト： FieldUpdateRequest→ FieldEntity
            CreateMap<FieldUpdateRequest, FieldEntity>()
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
