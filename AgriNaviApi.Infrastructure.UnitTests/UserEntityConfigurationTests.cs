using AgriNaviApi.Infrastructure.Persistence.Entities;
using AgriNaviApi.Infrastructure.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace AgriNaviApi.Infrastructure.Tests.Persistence.EntityConfigurations
{
    /// <summary>
    /// UserEntityConfiguration の Configure メソッドが Uuid カラムの型を "char(36)" に設定することを検証するテスト
    /// </summary>
    public class UserEntityConfigurationTests
    {
        /// <summary>
        /// Uuid プロパティのカラム型が "char(36)" であることを検証します。
        /// </summary>
        [Fact]
        public void Configure_SetsUuidColumnTypeToChar36()
        {
            // Arrange
            var builder = new ModelBuilder();
            var entityBuilder = builder.Entity<UserEntity>();
            var configuration = new UserEntityConfiguration();

            // Act
            configuration.Configure(entityBuilder);

            // Assert
            var uuidProperty = entityBuilder.Metadata.FindProperty(nameof(UserEntity.Uuid));
            Assert.NotNull(uuidProperty);
            Assert.Equal("char(36)", uuidProperty.GetColumnType());
        }
    }
}
