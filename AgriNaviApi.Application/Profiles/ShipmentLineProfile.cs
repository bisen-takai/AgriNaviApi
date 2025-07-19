using AgriNaviApi.Application.Requests.ShipmentLines;
using AgriNaviApi.Application.Responses.ShipmentLines;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 出荷記録詳細情報のマッピング
    /// </summary>
    public class ShipmentLineProfile : Profile
    {
        public ShipmentLineProfile()
        {
            #region Entity → Response
            // 登録レスポンス: Entity→ ShipmentLineCreateResponse
            CreateMap<ShipmentLineEntity, ShipmentLineCreateResponse>();

            // 詳細レスポンス：Entity→ ShipmentLineDetailResponse
            CreateMap<ShipmentLineEntity, ShipmentLineDetailResponse>();

            // 更新レスポンス：Entity→ ShipmentLineUpdateResponse
            CreateMap<ShipmentLineEntity, ShipmentLineUpdateResponse>();

            // 削除(手動でレスポンスを作成するため、マッピング不要)

            // 検索レスポンス：Entity→ ShipmentLineListItemResponse
            CreateMap<ShipmentLineEntity, ShipmentLineListItemResponse>();
            #endregion

            #region Request → Entity
            // 登録リクエスト： ShipmentLineCreateRequest →  ShipmentLineEntity
            CreateMap<ShipmentLineCreateRequest, ShipmentLineEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Shipment, opt => opt.Ignore())
                .ForMember(dest => dest.QualityStandard, opt => opt.Ignore())
                .ForMember(dest => dest.ShipDestination, opt => opt.Ignore())
                .ForMember(dest => dest.Unit, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新リクエスト： ShipmentLineUpdateRequest→ ShipmentLineEntity
            CreateMap<ShipmentLineUpdateRequest, ShipmentLineEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Shipment, opt => opt.Ignore())
                .ForMember(dest => dest.QualityStandard, opt => opt.Ignore())
                .ForMember(dest => dest.ShipDestination, opt => opt.Ignore())
                .ForMember(dest => dest.Unit, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
