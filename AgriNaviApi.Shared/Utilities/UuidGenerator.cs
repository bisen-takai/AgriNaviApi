using AgriNaviApi.Shared.Interfaces;

namespace AgriNaviApi.Shared.Utilities
{
    /// <summary>
    /// UUIDを生成する
    /// </summary>
    public class UuidGenerator : IUuidGenerator
    {
        /// <summary>
        /// UUIDを生成する
        /// </summary>
        /// <returns>新しい Guid を返します</returns>
        public Guid GenerateUuid()
        {
            return Guid.NewGuid();
        }
    }
}
