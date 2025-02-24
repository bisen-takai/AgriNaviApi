namespace AgriNaviApi.Common.Exceptions
{
    /// <summary>
    /// 登録済みの例外
    /// </summary>
    /// <remarks>
    /// 重複登録の例外を明示することが目的のため、特に処理は無し
    /// </remarks>
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(string message) : base(message) { }
    }
}
