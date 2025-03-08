using AgriNaviApi.Common.Interfaces;
using AgriNaviApi.Common.Settings;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace AgriNaviApi.Common.Utilities
{
    /// <summary>
    /// ソルト値を生成する
    /// </summary>
    public class SaltGenerator : ISaltGenerator
    {
        private readonly SecuritySetting _settings;

        public SaltGenerator(IOptions<SecuritySetting> options)
        {
            _settings = options.Value;
        }

        /// <summary>
        /// ソルト値を生成する
        /// </summary>
        /// <returns>ソルト値</returns>
        public string GenerateSalt()
        {
            byte[] salt = new byte[_settings.SaltSize];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }
    }
}
