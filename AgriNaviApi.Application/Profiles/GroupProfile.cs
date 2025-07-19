using AgriNaviApi.Application.Requests.Groups;
using AgriNaviApi.Application.Responses.Groups;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// グループ情報のマッピング
    /// </summary>
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            #region Entity → Response
            // 登録レスポンス: Entity→ GroupCreateResponse
            CreateMap<GroupEntity, GroupCreateResponse>();

            // 詳細レスポンス：Entity→ GroupDetailResponse
            CreateMap<GroupEntity, GroupDetailResponse>();

            // 更新レスポンス：Entity→ GroupUpdateResponse
            CreateMap<GroupEntity, GroupUpdateResponse>();

            // 削除(手動でレスポンスを作成するため、マッピング不要)

            // 検索レスポンス：Entity→ GroupListItemResponse
            CreateMap<GroupEntity, GroupListItemResponse>();
            #endregion

            #region Request → Entity
            // 登録リクエスト： GroupCreateRequest →  GroupEntity
            CreateMap<GroupCreateRequest, GroupEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新リクエスト： GroupUpdateRequest→ GroupEntity
            CreateMap<GroupUpdateRequest, GroupEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
