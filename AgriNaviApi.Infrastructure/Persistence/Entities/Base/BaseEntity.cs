using AgriNaviApi.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriNaviApi.Infrastructure.Persistence.Entities.Base
{
    /// <summary>
    /// 各エンティティ共通のカラムを定義
    /// </summary>
    public abstract class BaseEntity : IHasId
    {
        /// <summary>
        /// 主キー(自動インクリメント)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// 登録日時（UTC）
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// 最終更新日時（UTC）
        /// </summary>
        [Column("last_updated_at")]
        public DateTime LastUpdatedAt { get; private set; }
    }
}
