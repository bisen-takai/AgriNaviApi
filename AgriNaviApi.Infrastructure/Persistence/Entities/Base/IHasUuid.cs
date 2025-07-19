namespace AgriNaviApi.Infrastructure.Persistence.Entities.Base
{
    /// <summary>
    /// 外部公開用ID
    /// </summary>
    public interface IHasUuid
    {
        /// <summary>
        /// UUID
        /// </summary>
        Guid Uuid { get; }
    }
}
