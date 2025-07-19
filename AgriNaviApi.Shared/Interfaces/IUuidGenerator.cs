namespace AgriNaviApi.Shared.Interfaces
{
    /// <summary>
    /// UUIDを生成するインタフェース
    /// </summary>
    public interface IUuidGenerator
    {
        /// <summary>
        /// UUIDを生成する
        /// </summary>
        /// <returns>新しい Guid を返します</returns>
        Guid GenerateUuid();
    }
}
