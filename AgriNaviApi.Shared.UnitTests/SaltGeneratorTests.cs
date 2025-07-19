using AgriNaviApi.Shared.Interfaces;
using AgriNaviApi.Shared.Settings;
using AgriNaviApi.Shared.Utilities;
using Microsoft.Extensions.Options;

namespace AgriNaviApi.Shared.Tests.Utilities
{
    public class SaltGeneratorTests
    {
        /// <summary>
        /// ソルト生成メソッドが、指定した長さのBase64文字列を返すことを検証します。
        /// </summary>
        [Fact]
        public void GenerateSalt_ReturnsBase64String_WithExpectedLength()
        {
            // Arrange
            int saltSize = 32;
            var settings = new SaltSecuritySettings { SaltSize = saltSize };
            var options = Options.Create(settings);
            ISaltGenerator generator = new SaltGenerator(options);

            // Act
            string salt = generator.GenerateSalt();

            // Assert
            Assert.False(string.IsNullOrEmpty(salt));
            byte[] saltBytes = Convert.FromBase64String(salt);
            Assert.Equal(saltSize, saltBytes.Length);
        }

        /// <summary>
        /// ソルト生成メソッドが、毎回異なる値を返すことを検証します。
        /// </summary>
        [Fact]
        public void GenerateSalt_ReturnsDifferentValuesEachTime()
        {
            // Arrange
            int saltSize = 16;
            var settings = new SaltSecuritySettings { SaltSize = saltSize };
            var options = Options.Create(settings);
            ISaltGenerator generator = new SaltGenerator(options);

            // Act
            string salt1 = generator.GenerateSalt();
            string salt2 = generator.GenerateSalt();

            // Assert
            Assert.NotEqual(salt1, salt2);
        }
    }
}
