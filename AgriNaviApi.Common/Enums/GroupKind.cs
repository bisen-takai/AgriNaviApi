namespace AgriNaviApi.Common.Enums
{
    /// <summary>
    /// グループ名の種別
    /// </summary>
    public enum GroupKind
    {
        /// <summary>
        /// 未定義(デフォルト)
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// 圃場情報グループ
        /// </summary>
        Farm = 1,

        /// <summary>
        /// 作付名グループ
        /// </summary>
        Planting = 2,

        /// <summary>
        /// その他グループ
        /// </summary>
        Other = 3
    }
}
