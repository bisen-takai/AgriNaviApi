using AgriNaviApi.Application.DTOs.Fields;
using AgriNaviApi.Application.Requests.Fields;

namespace AgriNaviApi.Application.Interfaces
{
    /// <summary>
    /// 圃場テーブルに関するサービス処理のインタフェース
    /// </summary>
    public interface IFieldService
    {
        /// <summary>
        /// 圃場テーブルに登録する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<FieldCreateDto> CreateFieldAsync(FieldCreateRequest request);

        /// <summary>
        /// 圃場テーブル詳細を取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<FieldDetailDto> GetFieldByIdAsync(int id);

        /// <summary>
        /// 圃場テーブルを更新する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<FieldUpdateDto> UpdateFieldAsync(FieldUpdateRequest request);

        /// <summary>
        /// 圃場テーブルから削除する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<FieldDeleteDto> DeleteFieldAsync(FieldDeleteRequest request);

        /// <summary>
        /// 圃場テーブルを検索する
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<FieldSearchDto> SearchFieldAsync(FieldSearchRequest request);
    }
}
