using AgriNaviApi.Shared.Interfaces;
using AgriNaviApi.Shared.Settings;
using AgriNaviApi.Shared.Utilities;
using Microsoft.Extensions.Options;

namespace AgriNaviApi.Shared.Tests.Utilities
{
    public class SaltGeneratorTests
    {
        /// <summary>
        /// �\���g�������\�b�h���A�w�肵��������Base64�������Ԃ����Ƃ����؂��܂��B
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
        /// �\���g�������\�b�h���A����قȂ�l��Ԃ����Ƃ����؂��܂��B
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
