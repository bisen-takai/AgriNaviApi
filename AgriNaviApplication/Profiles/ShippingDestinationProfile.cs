using AgriNaviApi.Application.DTOs.ShippingDestinations;
using AgriNaviApi.Application.DTOs.Units;
using AgriNaviApi.Application.Requests.ShippingDestinations;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 出荷先名テーブルに関するマッピング管理
    /// </summary>
    public class ShippingDestinationProfile : Profile
    {
        public ShippingDestinationProfile()
        {
            #region Entity → Dto
            CreateMap<ShippingDestinationEntity, ShippingDestinationCreateDto>();

            CreateMap<ShippingDestinationEntity, ShippingDestinationUpdateDto>();

            CreateMap<ShippingDestinationEntity, ShippingDestinationDeleteDto>();

            CreateMap<ShippingDestinationEntity, ShippingDestinationDetailDto>();

            // 検索結果用
            CreateMap<ShippingDestinationEntity, ShippingDestinationListItemDto>();
            #endregion

            #region Request → Entity
            // 新規作成(Id, Uuid, CreatedAt, LastUpdatedAt は無視)
            CreateMap<ShippingDestinationCreateRequest, ShippingDestinationEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新(Id,UUIDは変更不可, LastUpdatedAtはサービス層で設定)
            CreateMap<ShippingDestinationUpdateRequest, ShippingDestinationEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)
            #endregion
        }
    }
}
