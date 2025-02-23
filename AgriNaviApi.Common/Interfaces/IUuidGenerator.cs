namespace AgriNaviApi.Common.Interfaces
{
    /// <summary>
    /// UUIDを生成するインタフェース
    /// </summary>
    public interface IUuidGenerator
    {
        /// <summary>
        /// UUIDを生成する
        /// </summary>
        /// <returns>UUID</returns>
        Guid GenerateUuid();
    }
}
