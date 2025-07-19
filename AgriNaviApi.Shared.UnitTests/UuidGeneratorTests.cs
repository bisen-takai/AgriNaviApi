using Xunit;
using AgriNaviApi.Shared.Utilities;
using System;

namespace AgriNaviApi.Shared.Tests.Utilities
{
    /// <summary>
    /// UuidGenerator.GenerateUuid() のテスト
    /// </summary>
    public class UuidGeneratorTests
    {
        /// <summary>
        /// GenerateUuid() が Guid.Empty 以外の有効な Guid を返すことを検証します。
        /// </summary>
        [Fact]
        public void GenerateUuid_ReturnsValidGuid()
        {
            // Arrange
            var generator = new UuidGenerator();

            // Act
            var uuid = generator.GenerateUuid();

            // Assert
            Assert.NotEqual(Guid.Empty, uuid);
        }

        /// <summary>
        /// GenerateUuid() が一意な Guid を返すことを検証します。
        /// </summary>
        [Fact]
        public void GenerateUuid_ReturnsUniqueGuids()
        {
            // Arrange
            var generator = new UuidGenerator();

            // Act
            var uuid1 = generator.GenerateUuid();
            var uuid2 = generator.GenerateUuid();

            // Assert
            Assert.NotEqual(uuid1, uuid2);
        }
    }
}
