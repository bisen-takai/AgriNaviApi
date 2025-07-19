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
        /// ����ȃp�X���[�h�ƃ\���g�Ńn�b�V���l����������邱�Ƃ����؂���
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
        /// �p�X���[�h����̏ꍇ�ɗ�O���X���[����邱�Ƃ����؂���
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
        /// �\���g����̏ꍇ�ɗ�O���X���[����邱�Ƃ����؂���
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
        /// �\���g�̃t�H�[�}�b�g���s���ȏꍇ�ɗ�O���X���[����邱�Ƃ����؂���
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
        /// �������p�X���[�h�Ō��؂����ꍇ��true���Ԃ邱�Ƃ����؂���
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
        /// �Ԉ�����p�X���[�h�Ō��؂����ꍇ��false���Ԃ邱�Ƃ����؂���
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
        /// �n�b�V���l����̏ꍇ�ɗ�O���X���[����邱�Ƃ����؂���
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
        /// �n�b�V���l�̃t�H�[�}�b�g���s���ȏꍇ�ɗ�O���X���[����邱�Ƃ����؂���
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
