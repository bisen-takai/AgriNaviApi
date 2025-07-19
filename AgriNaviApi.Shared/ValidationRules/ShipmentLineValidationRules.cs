namespace AgriNaviApi.Shared.ValidationRules
{
    /// <summary>
    /// 出荷記録詳細情報バリデーションルール
    /// </summary>
    public static class ShipmentLineValidationRules
    {
        /// <summary>
        /// 数量最小値
        /// </summary>
        public const int QuantityMin = 0;

        /// <summary>
        /// 数量最大値
        /// </summary>
        public const int QuantityMax = 99999;

        /// <summary>
        /// 金額最小値
        /// </summary>
        public const int AmountMin = 0;

        /// <summary>
        /// 金額最大値
        /// </summary>
        public const int AmountMax = 9999999;
    }
}
