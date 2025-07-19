using AgriNaviApi.Shared.Interfaces;
using AgriNaviApi.Shared.Settings;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace AgriNaviApi.Shared.Utilities
{
    public class SaltGenerator : ISaltGenerator
    {
        private readonly SaltSecuritySettings _settings;

        public SaltGenerator(IOptions<SaltSecuritySettings> options)
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
