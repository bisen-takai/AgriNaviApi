namespace AgriNaviApi.Shared.ValidationRules
{
    /// <summary>
    /// カラー情報のバリデーションルール
    /// </summary>
    public static class ColorValidationRules
    {
        /// <summary>
        /// カラー名の最大文字数
        /// </summary>
        public const int NameMax = 20;

        /// <summary>
        /// カラー値最小値
        /// </summary>
        public const int ColorValueMin = 0;

        /// <summary>
        /// カラー値最大値
        /// </summary>
        public const int ColorValueMax = 255;
    }
}
