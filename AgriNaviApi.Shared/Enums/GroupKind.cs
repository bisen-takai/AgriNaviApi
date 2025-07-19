using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Shared.Enums
{
    /// <summary>
    /// グループの種別
    /// </summary>
    public enum GroupKind : byte
    {
        /// <summary>
        /// 未定義(デフォルト)
        /// </summary>
        [Display(Name = "未定義")]
        Undefined = 0,

        /// <summary>
        /// 圃場情報グループ
        /// </summary>
        [Display(Name = "圃場")]
        Farm = 1,

        /// <summary>
        /// 作付名グループ
        /// </summary>
        [Display(Name = "作付")]
        Planting = 2
    }
}
