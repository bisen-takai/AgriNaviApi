using AgriNaviApi.Common.Interfaces;

namespace AgriNaviApi.Common.Utilities
{
    /// <summary>
    /// 現在のUTC日時を取得
    /// </summary>
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        /// <summary>
        /// 現在のUTC日時を取得
        /// </summary>
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
