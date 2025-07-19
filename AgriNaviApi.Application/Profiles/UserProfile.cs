using AgriNaviApi.Application.Requests.Users;
using AgriNaviApi.Application.Responses;
using AgriNaviApi.Application.Responses.Users;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// ユーザ情報のマッピング
    /// </summary>
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            #region Entity → Response
            CreateMap<ColorEntity, ColorResponse>();

            // 登録レスポンス: Entity→ UserCreateResponse
            CreateMap<UserEntity, UserCreateResponse>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

            // 詳細レスポンス：Entity→ UserDetailResponse
            CreateMap<UserEntity, UserDetailResponse>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

            // 更新レスポンス：Entity→ UserUpdateResponse
            CreateMap<UserEntity, UserUpdateResponse>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));
            CreateMap<UserEntity, PasswordUpdateResponse>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));

            // 削除(手動でレスポンスを作成するため、マッピング不要)

            // 検索レスポンス：Entity→ UserListItemResponse
            CreateMap<UserEntity, UserListItemResponse>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));
            CreateMap<UserEntity, UserLoginResponse>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color));
            #endregion

            #region Request → Entity
            // 登録リクエスト： UserCreateRequest →  UserEntity
            CreateMap<UserCreateRequest, UserEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Salt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Color, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新リクエスト： UserUpdateRequest→ UserEntity
            CreateMap<UserUpdateRequest, UserEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Salt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Color, opt => opt.Ignore());
            CreateMap<PasswordUpdateRequest, UserEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Salt, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Color, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
