namespace AgriNaviApi.Common.Interfaces
{
    /// <summary>
    /// ソルト値を生成するインタフェース
    /// </summary>
    public interface ISaltGenerator
    {
        /// <summary>
        /// ソルト値を生成する
        /// </summary>
        /// <returns>ソルト値</returns>
        string GenerateSalt();
    }
}
