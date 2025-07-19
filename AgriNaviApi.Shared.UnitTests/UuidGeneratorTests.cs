using Xunit;
using AgriNaviApi.Shared.Utilities;
using System;

namespace AgriNaviApi.Shared.Tests.Utilities
{
    /// <summary>
    /// UuidGenerator.GenerateUuid() �̃e�X�g
    /// </summary>
    public class UuidGeneratorTests
    {
        /// <summary>
        /// GenerateUuid() �� Guid.Empty �ȊO�̗L���� Guid ��Ԃ����Ƃ����؂��܂��B
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
        /// GenerateUuid() ����ӂ� Guid ��Ԃ����Ƃ����؂��܂��B
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
