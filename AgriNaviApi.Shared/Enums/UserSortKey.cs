namespace AgriNaviApi.Shared.Enums
{
    /// <summary>
    /// ユーザのソートキー
    /// </summary>
    public enum UserSortKey
    {
        /// <summary>
        /// ログインIDでソート
        /// </summary>
        LoginId = 0,

        /// <summary>
        /// 権限IDでソート
        /// </summary>
        PrivilegeId = 1,

        /// <summary>
        /// 氏名でソート
        /// </summary>
        FullName = 2
    }
}
