namespace AgriNaviApi.Common.Interfaces
{
    /// <summary>
    /// 現在のUTC日時を返すインタフェース
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// 現在のUTC日時を返す
        /// </summary>
        DateTime UtcNow { get; }
    }
}
