using AgriNaviApi.Application.Requests.ShipDestinations;
using AgriNaviApi.Application.Responses.ShipDestinations;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 出荷先情報のマッピング
    /// </summary>
    public class ShipDestinationProfile : Profile
    {
        public ShipDestinationProfile()
        {
            #region Entity → Response
            // 登録レスポンス: Entity→ ShipDestinationCreateResponse
            CreateMap<ShipDestinationEntity, ShipDestinationCreateResponse>();

            // 詳細レスポンス：Entity→ ShipDestinationDetailResponse
            CreateMap<ShipDestinationEntity, ShipDestinationDetailResponse>();

            // 更新レスポンス：Entity→ ShipDestinationUpdateResponse
            CreateMap<ShipDestinationEntity, ShipDestinationUpdateResponse>();

            // 削除(手動でレスポンスを作成するため、マッピング不要)

            // 検索レスポンス：Entity→ ShipDestinationListItemResponse
            CreateMap<ShipDestinationEntity, ShipDestinationListItemResponse>();
            #endregion

            #region Request → Entity
            // 登録リクエスト： ShipDestinationCreateRequest →  ShipDestinationEntity
            CreateMap<ShipDestinationCreateRequest, ShipDestinationEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新リクエスト： ShipDestinationUpdateRequest→ ShipDestinationEntity
            CreateMap<ShipDestinationUpdateRequest, ShipDestinationEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
