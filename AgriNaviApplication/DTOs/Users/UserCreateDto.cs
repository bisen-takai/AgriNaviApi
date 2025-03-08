using AgriNaviApi.Common.Enums;

namespace AgriNaviApi.Application.DTOs.Users
{
    /// <summary>
    /// ユーザ登録レスポンス
    /// </summary>
    public class UserCreateDto
    {
        /// <summary>
        /// ユーザID(自動インクリメントID)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ユーザUUID
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// ログインID
        /// </summary>
        public string LoginId { get; set; } = string.Empty;

        /// <summary>
        /// 氏名
        /// </summary>
        public string? FullName { get; set; }

        /// <summary>
        /// 電話番号
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 住所
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// 権限ID
        /// </summary>
        public PrivilegeKind PrivilegeId { get; set; }

        /// <summary>
        /// カラーID
        /// </summary>
        public int ColorId { get; set; }

        /// <summary>
        /// カラー名
        /// </summary>
        public string ColorName { get; set; } = string.Empty;

        /// <summary>
        /// Red値
        /// </summary>
        public int ColorRedValue { get; set; }

        /// <summary>
        /// Green値
        /// </summary>
        public int ColorGreenValue { get; set; }

        /// <summary>
        /// Blue値
        /// </summary>
        public int ColorBlueValue { get; set; }

        /// <summary>
        /// 備考
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 登録日時
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最終更新日時
        /// </summary>
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
