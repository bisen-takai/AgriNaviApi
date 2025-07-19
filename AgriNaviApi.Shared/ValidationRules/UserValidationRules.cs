namespace AgriNaviApi.Shared.ValidationRules
{
    /// <summary>
    /// ユーザ情報バリデーションルール
    /// </summary>
    public static class UserValidationRules
    {
        /// <summary>
        /// ログインIDの最大文字数
        /// </summary>
        public const int LoginIdMax = 20;

        /// <summary>
        /// パスワード最小文字数
        /// </summary>
        public const int PasswordNumMin = 8;

        /// <summary>
        /// パスワード最大文字数
        /// </summary>
        public const int PasswordNumMax = 32;

        /// <summary>
        /// パスワードハッシュ文字数
        /// </summary>
        public const int PasswordHashLen = 64;

        /// <summary>
        /// ソルト文字数
        /// </summary>
        public const int SaltLen = 24;

        /// <summary>
        /// 氏名の最大文字数
        /// </summary>
        public const int FullNameMax = 30;

        /// <summary>
        /// 住所の最大文字数
        /// </summary>
        public const int AddressMax = 100;

        /// <summary>
        /// 電話番号最小文字数
        /// </summary>
        public const int PhoneNumMin = 10;

        /// <summary>
        /// 電話番号最大文字数
        /// </summary>
        public const int PhoneNumMax = 11;

        /// <summary>
        /// 電話番号バリデーションルールの正規表現
        /// </summary>
        public const string PhoneNumberPattern = @"^\d{10,11}$";
    }
}
