namespace AgriNaviApi.Application.DTOs.Crops
{
    public class CropSearchDto
    {
        /// <summary>
        /// 検索結果のグループ一覧
        /// </summary>
        public IEnumerable<CropListItemDto> SearchItems { get; set; } = Enumerable.Empty<CropListItemDto>();

        /// <summary>
        /// 検索結果の件数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
