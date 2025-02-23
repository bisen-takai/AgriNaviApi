using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AgriNaviApi.Common.Enums;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// ユーザテーブル
    /// </summary>
    [Table("users")]
    public class UserEntity
    {
        /// <summary>
        /// ユーザID(自動インクリメントID)
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("user_id")]
        public int Id { get; set; }

        /// <summary>
        /// ユーザUUID
        /// </summary>
        [Column("user_uuid")]
        public Guid Uuid { get; set; }

        /// <summary>
        /// ログインID
        /// </summary>
        [Column("user_login_id")]
        [Required]
        [StringLength(20)]
        public string LoginId { get; set; } = string.Empty;

        /// <summary>
        /// ハッシュ化したパスワード
        /// </summary>
        [Column("user_password")]
        [Required]
        [StringLength(64, MinimumLength = 64)]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// ソルト値
        /// </summary>
        [Column("user_salt")]
        [Required]
        [StringLength(24, MinimumLength = 24)]
        public string Salt { get; set; } = string.Empty;

        /// <summary>
        /// 氏名
        /// </summary>
        [Column("user_full_name")]
        [MaxLength(20)]
        public string? FullName { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        [Column("user_phone_number")]
        [RegularExpression(@"^\d{10,11}$")]
        [StringLength(11)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        [Column("user_email")]
        [EmailAddress]
        public string? Email { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        [Column("user_address")]
        public string? Address { get; set; }

        /// <summary>
        /// 権限ID
        /// </summary>
        [Column("privilege_id")]
        public PrivilegeKind PrivilegeId { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        [Column("color_id")]
        public int ColorId { get; set; }

        /// <summary>
        /// カラーエンティティ
        /// </summary>
        [ForeignKey(nameof(ColorId))]
        public ColorEntity Color { get; set; } = null!;

        /// <summary>
        /// 備考
        /// </summary>
        [Column("user_remark")]
        [StringLength(200)]
        public string? Remark { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("user_delete_flg")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最終更新日時
        /// </summary>
        [Column("last_updated_at")]
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;


        /// <summary>
        /// EF Coreマッピング用
        /// </summary>
        public UserEntity()
        {
        }

        /// <summary>
        /// 非null許容型の外部キーのエンティティの初期値を設定する
        /// </summary>
        /// <param name="color">カラーエンティティ</param>
        /// <exception cref="ArgumentNullException"></exception>
        public UserEntity(ColorEntity color)
        {
            Color = color ?? throw new ArgumentNullException(nameof(color));
        }
    }
}
