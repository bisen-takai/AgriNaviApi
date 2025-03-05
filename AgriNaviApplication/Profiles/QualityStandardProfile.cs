using AgriNaviApi.Application.DTOs.QualityStandards;
using AgriNaviApi.Application.Requests.QualityStandards;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 品質・規格テーブルに関するマッピング管理
    /// </summary>
    public class QualityStandardProfile : Profile
    {
        public QualityStandardProfile()
        {
            #region Entity → Dto
            CreateMap<QualityStandardEntity, QualityStandardCreateDto>();

            CreateMap<QualityStandardEntity, QualityStandardUpdateDto>();

            CreateMap<QualityStandardEntity, QualityStandardDeleteDto>();

            CreateMap<QualityStandardEntity, QualityStandardDetailDto>();
            #endregion

            #region Request → Entity
            // 新規作成(Id, Uuid, CreatedAt, LastUpdatedAt は無視)
            CreateMap<QualityStandardCreateRequest, QualityStandardEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新(Id,UUIDは変更不可, LastUpdatedAtはサービス層で設定)
            CreateMap<QualityStandardUpdateRequest, QualityStandardEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Uuid, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)
            #endregion
        }
    }
}
