using AgriNaviApi.Shared.Enums;

namespace AgriNaviApi.Application.Responses.Users
{
    /// <summary>
    /// ユーザレスポンスの基底クラス
    /// </summary>
    public abstract record UserBaseResponse
    {
        /// <summary>
        /// ユーザUUID
        /// </summary>
        public Guid Uuid { get; init; }

        /// <summary>
        /// ログインID
        /// </summary>
        public string LoginId { get; init; } = string.Empty;

        /// <summary>
        /// 氏名
        /// </summary>
        public string? FullName { get; init; }

        /// <summary>
        /// 電話番号
        /// </summary>
        public string? PhoneNumber { get; init; }

        /// <summary>
        /// メールアドレス
        /// </summary>
        public string? Email { get; init; }

        /// <summary>
        /// 住所
        /// </summary>
        public string? Address { get; init; }

        /// <summary>
        /// 権限ID
        /// </summary>
        public PrivilegeKind Privilege { get; init; }

        /// <summary>
        /// カラー情報
        /// </summary>
        public required ColorResponse Color { get; init; }

        /// <summary>
        /// 備考
        /// </summary>
        public string? Remark { get; init; }
    }
}
