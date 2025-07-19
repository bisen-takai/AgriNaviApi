using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Shared.Enums
{
    /// <summary>
    /// 検索一致タイプ
    /// </summary>
    public enum SearchMatchType : byte
    {
        /// <summary>
        /// 指定なし
        /// </summary>
        [Display(Name = "未定義")]
        None = 0,

        /// <summary>
        /// 完全一致
        /// </summary>
        [Display(Name = "完全一致")]
        Exact = 1,

        /// <summary>
        /// 前方一致
        /// </summary>
        [Display(Name = "前方一致")]
        Prefix = 2,

        /// <summary>
        /// 後方一致
        /// </summary>
        [Display(Name = "後方一致")]
        Suffix = 3,

        /// <summary>
        /// 部分一致
        /// </summary>
        [Display(Name = "部分一致")]
        Partial = 4
    }
}
