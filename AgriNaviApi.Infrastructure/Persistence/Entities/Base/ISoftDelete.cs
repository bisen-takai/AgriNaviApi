using System.ComponentModel.DataAnnotations.Schema;

namespace AgriNaviApi.Infrastructure.Persistence.Entities.Base
{
    /// <summary>
    /// 論理削除
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// 削除あり
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// 削除日時（UTC）
        /// </summary>
        DateTime? DeletedAt { get; set; }
    }
}
