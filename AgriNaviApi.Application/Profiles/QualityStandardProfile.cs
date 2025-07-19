using AgriNaviApi.Application.Requests.QualityStandards;
using AgriNaviApi.Application.Responses.QualityStandards;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AutoMapper;

namespace AgriNaviApi.Application.Profiles
{
    /// <summary>
    /// 品質・規格情報のマッピング
    /// </summary>
    public class QualityStandardProfile : Profile
    {
        public QualityStandardProfile()
        {
            #region Entity → Response
            // 登録レスポンス: Entity→ QualityStandardCreateResponse
            CreateMap<QualityStandardEntity, QualityStandardCreateResponse>();

            // 詳細レスポンス：Entity→ QualityStandardDetailResponse
            CreateMap<QualityStandardEntity, QualityStandardDetailResponse>();

            // 更新レスポンス：Entity→ QualityStandardUpdateResponse
            CreateMap<QualityStandardEntity, QualityStandardUpdateResponse>();

            // 削除(手動でレスポンスを作成するため、マッピング不要)

            // 検索レスポンス：Entity→ QualityStandardListItemResponse
            CreateMap<QualityStandardEntity, QualityStandardListItemResponse>();
            #endregion

            #region Request → Entity
            // 登録リクエスト： QualityStandardCreateRequest →  QualityStandardEntity
            CreateMap<QualityStandardCreateRequest, QualityStandardEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 詳細(Idを受け取るのみなので、マッピング不要)

            // 更新リクエスト： QualityStandardUpdateRequest→ QualityStandardEntity
            CreateMap<QualityStandardUpdateRequest, QualityStandardEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastUpdatedAt, opt => opt.Ignore());

            // 削除(Idを受け取るのみなので、マッピング不要)

            // 検索(検索条件のみで、エンティティの作成更新しないため、マッピング不要)
            #endregion
        }
    }
}
