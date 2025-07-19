namespace AgriNaviApi.Application.Responses.Crops
{
    /// <summary>
    /// 作付詳細取得時のレスポンス
    /// </summary>
    public record CropDetailResponse : CropBaseResponse
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
