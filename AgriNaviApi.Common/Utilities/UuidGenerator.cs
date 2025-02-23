using AgriNaviApi.Common.Interfaces;

namespace AgriNaviApi.Common.Utilities
{
    /// <summary>
    /// UUIDを生成する
    /// </summary>
    public class UuidGenerator : IUuidGenerator
    {
        /// <summary>
        /// UUIDを生成する
        /// </summary>
        /// <returns>UUID</returns>
        public Guid GenerateUuid()
        {
            return Guid.NewGuid();
        }
    }
}
