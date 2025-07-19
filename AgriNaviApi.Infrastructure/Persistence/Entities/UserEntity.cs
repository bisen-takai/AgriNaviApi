using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AgriNaviApi.Shared.Enums;
using AgriNaviApi.Infrastructure.Persistence.Entities.Base;
using Microsoft.EntityFrameworkCore;
using AgriNaviApi.Shared.ValidationRules;

namespace AgriNaviApi.Infrastructure.Persistence.Entities
{
    /// <summary>
    /// ユーザテーブル
    /// </summary>
    [Table("users")]
    [Index(nameof(LoginId), IsUnique = true)]
    [Index(nameof(Uuid), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class UserEntity : BaseEntity, IHasUuid, ISoftDelete
    {
        /// <summary>
        /// ユーザUUID（アプリ側から直接設定不可。SaveChanges内で自動設定）
        /// </summary>
        [Column("user_uuid")]
        public Guid Uuid { get; private set; }

        /// <summary>
        /// ログインID
        /// </summary>
        [Column("user_login_id")]
        [Required]
        [MaxLength(UserValidationRules.LoginIdMax)]
        public string LoginId { get; set; } = string.Empty;

        /// <summary>
        /// ハッシュ化したパスワード
        /// </summary>
        [Column("user_password")]
        [Required]
        [StringLength(UserValidationRules.PasswordHashLen, MinimumLength = UserValidationRules.PasswordHashLen)]
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// ソルト値
        /// </summary>
        [Column("user_salt")]
        [Required]
        [StringLength(UserValidationRules.SaltLen, MinimumLength = UserValidationRules.SaltLen)]
        public string Salt { get; set; } = string.Empty;

        /// <summary>
        /// 氏名
        /// </summary>
        [Column("user_full_name")]
        [MaxLength(UserValidationRules.FullNameMax)]
        public string? FullName { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        [Column("user_phone_number")]
        [RegularExpression(UserValidationRules.PhoneNumberPattern)]
        [MaxLength(UserValidationRules.PhoneNumMax)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        [Column("user_email")]
        [EmailAddress]
        [MaxLength(CommonValidationRules.EmailMax)]
        public string? Email { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        [Column("user_address")]
        [MaxLength(UserValidationRules.AddressMax)]
        public string? Address { get; set; }

        /// <summary>
        /// 権限ID
        /// </summary>
        [Column("privilege_id")]
        [EnumDataType(typeof(PrivilegeKind))]
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
        [MaxLength(CommonValidationRules.RemarkMax)]
        public string? Remark { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        [Column("delete_flg")]
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// 削除日時（UTC）
        /// </summary>
        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// EF Coreマッピング用
        /// </summary>
        [Obsolete("このコンストラクタはEF Coreが内部的に使用します。アプリケーションコードでの使用は避けてください。", error: false)]
        public UserEntity() { }

        /// <summary>
        /// 非null許容型の外部キーのエンティティの初期値を設定する
        /// </summary>
        /// <param name="color">カラーエンティティ</param>
        /// <exception cref="ArgumentNullException"></exception>
        public UserEntity(ColorEntity color)
        {
            Color = color ?? throw new ArgumentNullException(nameof(color));
            ColorId = Color.Id;
        }
    }
}
