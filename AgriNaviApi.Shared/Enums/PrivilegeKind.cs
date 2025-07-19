using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Shared.Enums
{
    /// <summary>
    /// 権限の種別
    /// </summary>
    public enum PrivilegeKind : byte
    {
        /// <summary>
        /// 未設定
        /// </summary>
        [Display(Name = "未設定")]
        None = 0,

        /// <summary>
        /// 管理者
        /// </summary>
        [Display(Name = "管理者")]
        Admin = 1,

        /// <summary>
        /// 一般
        /// </summary>
        [Display(Name = "一般")]
        User = 2,

        /// <summary>
        /// ゲスト
        /// </summary>
        [Display(Name = "ゲスト")]
        Guest = 3
    }
}
