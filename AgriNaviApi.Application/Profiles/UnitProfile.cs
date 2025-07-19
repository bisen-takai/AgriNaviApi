using AgriNaviApi.Application.Requests.Units;
using AgriNaviApi.Application.Responses.Units;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 単位情報のマッピング
    /// </summary>
    public class UnitProfile : Profile
    {
        public UnitProfile()
        {
            #region Entity → Response
            // 登録レスポンス: Entity→ UnitCreateResponse
            CreateMap<UnitEntity, UnitCreateResponse>();

            // 詳細レスポンス：Entity→ UnitDetailResponse
            CreateMap<UnitEntity, UnitDetailResponse>();

            // 更新レスポンス：Entity→ UnitUpdateResponse
            CreateMap<UnitEntity, UnitUpdateResponse>();

            // 削除(手動でレスポンスを作成するため、マッピング不要)

            // 検索レスポンス：Entity→ UnitListItemResponse
            CreateMap<UnitEntity, UnitListItemResponse>();
            #endregion

            #region Request → Entity
            // 登録リクエスト： UnitCreateRequest →  UnitEntity
            CreateMap<UnitCreateRequest, UnitEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新リクエスト： UnitUpdateRequest→ UnitEntity
            CreateMap<UnitUpdateRequest, UnitEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
