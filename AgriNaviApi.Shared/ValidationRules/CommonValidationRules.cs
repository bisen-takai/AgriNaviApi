namespace AgriNaviApi.Shared.ValidationRules
{
    /// <summary>
    /// 共通のバリデーションルール
    /// </summary>
    public static class CommonValidationRules
    {
        /// <summary>
        /// ID未定後を表す値
        /// </summary>
        public const int UndefinedId = 0;

        /// <summary>
        /// 効な実在IDの最小値
        /// </summary>
        public const int MinValidId = 1;

        /// <summary>
        /// カレンダーなどに表示する省略名の最大文字数
        /// </summary>
        public const int ShortNameMax = 5;

        /// <summary>
        /// 備考欄の最大文字数
        /// </summary>
        public const int RemarkMax = 300;

        /// <summary>
        /// メールアドレス最大文字数
        /// </summary>
        public const int EmailMax = 254;

        /// <summary>
        /// 1ページあたりの最小表示件数
        /// </summary>
        public const int PageSizeMin = 1;

        /// <summary>
        /// 1ページあたりの最大表示件数
        /// </summary>
        public const int PageSizeMax = 50;
    }
}
