namespace AgriNaviApi.Application.Responses.QualityStandards
{
    /// <summary>
    /// 品質・規格詳細取得時のレスポンス
    /// </summary>
    public record QualityStandardDetailResponse : QualityStandardBaseResponse
    {
        /// <summary>
        /// 登録日時
        /// </summary>
        public DateTime CreatedAt { get; init; }

        /// <summary>
        /// 最終更新日時
        /// </summary>
        public DateTime LastUpdatedAt { get; init; }
    }
}
