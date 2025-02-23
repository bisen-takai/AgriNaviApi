namespace AgriNaviApi.Common.Enums
{
    /// <summary>
    /// 権限の種別
    /// </summary>
    public enum PrivilegeKind
    {
        /// <summary>
        /// 未設定
        /// </summary>
        None = 0,

        /// <summary>
        /// 管理者
        /// </summary>
        Admin = 1,

        /// <summary>
        /// 一般
        /// </summary>
        User = 2,

        /// <summary>
        /// ゲスト
        /// </summary>
        Guest = 3
    }
}
