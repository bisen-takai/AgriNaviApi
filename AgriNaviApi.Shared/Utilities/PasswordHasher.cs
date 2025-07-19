using AgriNaviApi.Shared.Exceptions;
using AgriNaviApi.Shared.Interfaces;
using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.Settings;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace AgriNaviApi.Shared.Utilities
{
    /// <summary>
    /// ハッシュ値のパスワードに関するクラス
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordSecuritySettings _settings;

        public PasswordHasher(IOptions<PasswordSecuritySettings> options)
        {
            _settings = options.Value;
        }

        /// <summary>
        /// パスワードとソルト値を元にハッシュ値のパスワードを生成する
        /// </summary>
        /// <param name="password">パスワード</param>
        /// <param name="salt">ソルト値</param>
        /// <returns>ハッシュ値のパスワード</returns>
        public string HashPassword(string password, string salt)
        {
            ValidateNonEmptyInput(password, nameof(password));
            ValidateNonEmptyInput(salt, nameof(salt));

            try
            {
                byte[] saltBytes = Convert.FromBase64String(salt);
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, _settings.Iterations, HashAlgorithmName.SHA256))
                {
                    byte[] hashBytes = pbkdf2.GetBytes(_settings.KeySize);
                    return Convert.ToBase64String(hashBytes);
                }
            }
            catch (FormatException ex)
            {
                throw new PasswordHashingException(
                    resourceType: typeof(UserErrorMessages),
                    resourceKey: nameof(UserErrorMessages.SaltErrorMessage),
                    args: new[] { "salt" }
                );
            }

            catch (Exception ex)
            {
                throw new PasswordHashingException(
                    resourceType: typeof(UserErrorMessages),
                    resourceKey: nameof(UserErrorMessages.PasswordHashErrorMessage),
                    args: new[] { ex.GetType().Name }
                );
            }
        }

        /// <summary>
        /// パスワードとソルト値から生成したハッシュ値と引数のハッシュ値が一致しているかを確認する
        /// </summary>
        /// <param name="password">パスワード</param>
        /// <param name="salt">ソルト値</param>
        /// <param name="hash">ハッシュ値のパスワード</param>
        /// <returns>true：一致、false：不一致</returns>
        public bool VerifyPassword(string password, string salt, string hash)
        {
            ValidateNonEmptyInput(password, nameof(password));
            ValidateNonEmptyInput(salt, nameof(salt));
            ValidateNonEmptyInput(hash, nameof(hash));

            var computedHash = HashPassword(password, salt);

            byte[] hashBytes = Convert.FromBase64String(hash);
            byte[] computedHashBytes = Convert.FromBase64String(computedHash);

            return CryptographicOperations.FixedTimeEquals(hashBytes, computedHashBytes);
        }

        private void ValidateNonEmptyInput(string input, string paramName)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new PasswordHashingException(
                    resourceType: typeof(CommonErrorMessages),
                    resourceKey: nameof(CommonErrorMessages.InvalidInputErrorMessage)
                );
            }
        }
    }
}
