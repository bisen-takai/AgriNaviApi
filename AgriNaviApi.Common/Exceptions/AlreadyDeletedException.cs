namespace AgriNaviApi.Common.Exceptions
{
    /// <summary>
    /// 削除済みの例外
    /// </summary>
    /// <remarks>
    /// 削除済みの例外を明示することが目的のため、特に処理は無し
    /// </remarks>
    public class AlreadyDeletedException : InvalidOperationException
    {
        public AlreadyDeletedException(string message) : base(message) { }
    }
}
