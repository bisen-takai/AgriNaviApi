using AgriNaviApi.Application.Requests.SeasonSchedules;
using AgriNaviApi.Application.Responses.SeasonSchedules;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 作付計画情報のマッピング
    /// </summary>
    public class SeasonScheduleProfile : Profile
    {
        public SeasonScheduleProfile()
        {
            #region Entity → Response
            // 登録レスポンス: Entity→ SeasonScheduleCreateResponse
            CreateMap<SeasonScheduleEntity, SeasonScheduleCreateResponse>();

            // 詳細レスポンス：Entity→ SeasonScheduleDetailResponse
            CreateMap<SeasonScheduleEntity, SeasonScheduleDetailResponse>();

            // 更新レスポンス：Entity→ SeasonScheduleUpdateResponse
            CreateMap<SeasonScheduleEntity, SeasonScheduleUpdateResponse>();

            // 削除(手動でレスポンスを作成するため、マッピング不要)

            // 検索レスポンス：Entity→ SeasonScheduleListItemResponse
            CreateMap<SeasonScheduleEntity, SeasonScheduleListItemResponse>();
            #endregion

            #region Request → Entity
            // 登録リクエスト： SeasonScheduleCreateRequest →  SeasonScheduleEntity
            CreateMap<SeasonScheduleCreateRequest, SeasonScheduleEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Crop, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新リクエスト： SeasonScheduleUpdateRequest→ SeasonScheduleEntity
            CreateMap<SeasonScheduleUpdateRequest, SeasonScheduleEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Crop, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
