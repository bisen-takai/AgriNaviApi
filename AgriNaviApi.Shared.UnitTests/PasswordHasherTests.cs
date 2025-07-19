using AgriNaviApi.Shared.Exceptions;
using AgriNaviApi.Shared.Interfaces;
using AgriNaviApi.Shared.Resources;
using AgriNaviApi.Shared.Settings;
using AgriNaviApi.Shared.Utilities;
using Microsoft.Extensions.Options;
using Xunit;

namespace AgriNaviApi.Shared.Tests.Utilities
{
    public class PasswordHasherTests
    {
        private static PasswordSecuritySettings DefaultSettings => new PasswordSecuritySettings
        {
            Iterations = 10000,
            KeySize = 32
        };

        private static IPasswordHasher CreateHasher(PasswordSecuritySettings? settings = null)
        {
            var opts = Options.Create(settings ?? DefaultSettings);
            return new PasswordHasher(opts);
        }

        private static string GenerateSalt(int size = 16)
        {
            var bytes = new byte[size];
            Random.Shared.NextBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 正常なパスワードとソルトでハッシュ値が生成されることを検証する
        /// </summary>
        [Fact]
        public void HashPassword_ValidInput_ReturnsHash()
        {
            var hasher = CreateHasher();
            var password = "TestPassword123!";
            var salt = GenerateSalt();

            var hash = hasher.HashPassword(password, salt);

            Assert.False(string.IsNullOrWhiteSpace(hash));
        }

        /// <summary>
        /// パスワードが空の場合に例外がスローされることを検証する
        /// </summary>
        [Fact]
        public void HashPassword_EmptyPassword_ThrowsException()
        {
            var hasher = CreateHasher();
            var salt = GenerateSalt();

            var ex = Assert.Throws<PasswordHashingException>(() => hasher.HashPassword("", salt));
            Assert.Equal(typeof(CommonErrorMessages), ex.ResourceType);
            Assert.Equal(nameof(CommonErrorMessages.InvalidInputErrorMessage), ex.ResourceKey);
        }

        /// <summary>
        /// ソルトが空の場合に例外がスローされることを検証する
        /// </summary>
        [Fact]
        public void HashPassword_EmptySalt_ThrowsException()
        {
            var hasher = CreateHasher();
            var password = "TestPassword123!";

            var ex = Assert.Throws<PasswordHashingException>(() => hasher.HashPassword(password, ""));
            Assert.Equal(typeof(CommonErrorMessages), ex.ResourceType);
            Assert.Equal(nameof(CommonErrorMessages.InvalidInputErrorMessage), ex.ResourceKey);
        }

        /// <summary>
        /// ソルトのフォーマットが不正な場合に例外がスローされることを検証する
        /// </summary>
        [Fact]
        public void HashPassword_InvalidSaltFormat_ThrowsException()
        {
            var hasher = CreateHasher();
            var password = "TestPassword123!";
            var salt = "invalid_base64";

            var ex = Assert.Throws<PasswordHashingException>(() => hasher.HashPassword(password, salt));
            Assert.Equal(typeof(UserErrorMessages), ex.ResourceType);
            Assert.Equal(nameof(UserErrorMessages.SaltErrorMessage), ex.ResourceKey);
        }

        /// <summary>
        /// 正しいパスワードで検証した場合にtrueが返ることを検証する
        /// </summary>
        [Fact]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            var hasher = CreateHasher();
            var password = "TestPassword123!";
            var salt = GenerateSalt();
            var hash = hasher.HashPassword(password, salt);

            var result = hasher.VerifyPassword(password, salt, hash);

            Assert.True(result);
        }

        /// <summary>
        /// 間違ったパスワードで検証した場合にfalseが返ることを検証する
        /// </summary>
        [Fact]
        public void VerifyPassword_WrongPassword_ReturnsFalse()
        {
            var hasher = CreateHasher();
            var password = "TestPassword123!";
            var salt = GenerateSalt();
            var hash = hasher.HashPassword(password, salt);

            var result = hasher.VerifyPassword("WrongPassword", salt, hash);

            Assert.False(result);
        }

        /// <summary>
        /// ハッシュ値が空の場合に例外がスローされることを検証する
        /// </summary>
        [Fact]
        public void VerifyPassword_EmptyHash_ThrowsException()
        {
            var hasher = CreateHasher();
            var password = "TestPassword123!";
            var salt = GenerateSalt();

            var ex = Assert.Throws<PasswordHashingException>(() => hasher.VerifyPassword(password, salt, ""));
            Assert.Equal(typeof(CommonErrorMessages), ex.ResourceType);
            Assert.Equal(nameof(CommonErrorMessages.InvalidInputErrorMessage), ex.ResourceKey);
        }

        /// <summary>
        /// ハッシュ値のフォーマットが不正な場合に例外がスローされることを検証する
        /// </summary>
        [Fact]
        public void VerifyPassword_InvalidHashFormat_ThrowsException()
        {
            var hasher = CreateHasher();
            var password = "TestPassword123!";
            var salt = GenerateSalt();
            var hash = "invalid_base64";

            var ex = Assert.Throws<PasswordHashingException>(() => hasher.VerifyPassword(password, salt, hash));
            Assert.Equal(typeof(UserErrorMessages), ex.ResourceType);
            Assert.Equal(nameof(UserErrorMessages.PasswordHashErrorMessage), ex.ResourceKey);
        }
    }
}
