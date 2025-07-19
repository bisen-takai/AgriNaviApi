using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace AgriNaviApi.Shared.Utilities
{
    /// <summary>
    ///  DbUpdateException の中身を見て、各プロバイダの「ユニーク制約違反」を判定する共通ロジック
    /// </summary>
    public static class DbExceptionHelper
    {
        public static bool IsUniqueConstraintViolation(this DbUpdateException ex)
        {
            if (ex == null) return false;

            if (ex.InnerException is MySqlException mySqlEx)
            {
                // MySQL の一意制約違反エラー番号は 1062
                return mySqlEx.Number == 1062;
            }

            // 将来的に他のプロバイダーもサポート
            // if (ex.InnerException is SqlException sqlEx) { ... }
            // if (ex.InnerException is NpgsqlException pgEx) { ... }

            return false;
        }
    }
}
