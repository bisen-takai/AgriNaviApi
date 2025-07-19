namespace AgriNaviApi.Shared.ValidationRules
{
    /// <summary>
    /// 圃場情報のバリデーションルール
    /// </summary>
    public static class FieldValidationRules
    {
        /// <summary>
        /// 圃場名の最大文字数
        /// </summary>
        public const int NameMax = 30;

        /// <summary>
        /// 面積(m2)の最小値
        /// </summary>
        public const string AreaM2Min = "0";

        /// <summary>
        /// 面積(m2)の最大値
        /// </summary>
        public const string AreaM2Max = "9999999";
    }
}
