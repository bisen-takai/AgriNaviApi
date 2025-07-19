using AgriNaviApi.Application.Requests.Shipments;
using AgriNaviApi.Application.Responses.Shipments;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 出荷記録情報のマッピング
    /// </summary>
    public class ShipmentProfile : Profile
    {
        public ShipmentProfile()
        {
            #region Entity → Response
            // 登録レスポンス: Entity→ ShipmentCreateResponse
            CreateMap<ShipmentEntity, ShipmentCreateResponse>();

            // 詳細レスポンス：Entity→ ShipmentDetailResponse
            CreateMap<ShipmentEntity, ShipmentDetailResponse>();

            // 更新レスポンス：Entity→ ShipmentUpdateResponse
            CreateMap<ShipmentEntity, ShipmentUpdateResponse>();

            // 削除(手動でレスポンスを作成するため、マッピング不要)

            // 検索レスポンス：Entity→ ShipmentListItemResponse
            CreateMap<ShipmentEntity, ShipmentListItemResponse>();
            #endregion

            #region Request → Entity
            // 登録リクエスト： ShipmentCreateRequest →  ShipmentEntity
            CreateMap<ShipmentCreateRequest, ShipmentEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Field, opt => opt.Ignore())
                .ForMember(dest => dest.SeasonSchedule, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新リクエスト： ShipmentUpdateRequest→ ShipmentEntity
            CreateMap<ShipmentUpdateRequest, ShipmentEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Field, opt => opt.Ignore())
                .ForMember(dest => dest.SeasonSchedule, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
