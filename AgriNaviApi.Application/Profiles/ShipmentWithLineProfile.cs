using AgriNaviApi.Application.Requests.ShipmentWithLines;
using AgriNaviApi.Application.Responses.ShipmentWithLines;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 出荷記録情報のマッピング
    /// </summary>
    public class ShipmentWithLineProfile : Profile
    {
        public ShipmentWithLineProfile()
        {
            #region Entity → Response
            // 登録レスポンス: Entity→ ShipmentWithLineCreateResponse
            CreateMap<ShipmentEntity, ShipmentWithLineCreateResponse>();

            // 詳細レスポンス：Entity→ ShipmentWithLineDetailResponse
            CreateMap<ShipmentEntity, ShipmentWithLineDetailResponse>();

            // 更新レスポンス：Entity→ ShipmentWithLineUpdateResponse
            CreateMap<ShipmentEntity, ShipmentWithLineUpdateResponse>();

            // 削除(手動でレスポンスを作成するため、マッピング不要)

            // 検索レスポンス：Entity→ ShipmentWithLineListItemResponse
            CreateMap<ShipmentEntity, ShipmentWithLineListItemResponse>();
            #endregion

            #region Request → Entity
            // 登録リクエスト： ShipmentWithLineCreateRequest →  ShipmentEntity
            CreateMap<ShipmentWithLineCreateRequest, ShipmentEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Field, opt => opt.Ignore())
                .ForMember(dest => dest.SeasonSchedule, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新リクエスト： ShipmentWithLineUpdateRequest→ ShipmentEntity
            CreateMap<ShipmentWithLineUpdateRequest, ShipmentEntity>()
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
