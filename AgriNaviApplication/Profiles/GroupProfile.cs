using AgriNaviApi.Application.DTOs.Groups;
using AgriNaviApi.Application.Requests.Groups;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// グループテーブルに関するマッピング管理
    /// </summary>
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            #region Entity → Dto
            CreateMap<GroupEntity, GroupCreateDto>();

            CreateMap<GroupEntity, GroupUpdateDto>();

            CreateMap<GroupEntity, GroupDeleteDto>();

            CreateMap<GroupEntity, GroupDetailDto>();

            // 検索結果用
            CreateMap<GroupEntity, GroupListItemDto>();

            #endregion

            #region Request → Entity
            // 新規作成(Id, Uuid, CreatedAt, LastUpdatedAt は無視)
            CreateMap<GroupCreateRequest, GroupEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新(Id,UUIDは変更不可, LastUpdatedAtはサービス層で設定)
            CreateMap<GroupUpdateRequest, GroupEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
