using AgriNaviApi.Infrastructure.Persistence.Contexts;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AgriNaviApi.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AgriNaviApi.Infrastructure.Tests.Persistence.Contexts
{
    public class AppDbContextTests
    {
        private AppDbContext CreateDbContext(Mock<IUuidGenerator> uuidGeneratorMock)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options, uuidGeneratorMock.Object);
        }

        /// <summary>
        /// 新規追加したUserEntityに対してUUIDとタイムスタンプが正しく設定されることを検証する
        /// </summary>
        [Fact]
        public async Task SaveChangesAsync_AddedEntity_SetsUuidAndTimestamps()
        {
            // Arrange
            var uuid = Guid.NewGuid();
            var uuidGeneratorMock = new Mock<IUuidGenerator>();
            uuidGeneratorMock.Setup(x => x.GenerateUuid()).Returns(uuid);
            var dbContext = CreateDbContext(uuidGeneratorMock);

            var color = new ColorEntity { Name = "Red", Red = 255, Green = 0, Blue = 0 };
            dbContext.Colors.Add(color);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var user = new UserEntity(color)
            {
                LoginId = "testuser",
                PasswordHash = new string('a', 64),
                Salt = new string('b', 32),
                PrivilegeId = 0,
                ColorId = color.Id
            };
            dbContext.Users.Add(user);

            // Act
            await dbContext.SaveChangesAsync(CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, user.Uuid);
            Assert.True(user.CreatedAt <= user.LastUpdatedAt);
            uuidGeneratorMock.Verify(x => x.GenerateUuid(), Times.Once);
        }

        /// <summary>
        /// 既存UserEntityを更新した際、CreatedAtは変更されずLastUpdatedAtのみ更新されることを検証する
        /// </summary>
        [Fact]
        public async Task SaveChangesAsync_ModifiedEntity_UpdatesLastUpdatedAtOnly()
        {
            // Arrange
            var uuidGeneratorMock = new Mock<IUuidGenerator>();
            uuidGeneratorMock.Setup(x => x.GenerateUuid()).Returns(Guid.NewGuid());
            var dbContext = CreateDbContext(uuidGeneratorMock);

            var color = new ColorEntity { Name = "Blue", Red = 0, Green = 0, Blue = 255 };
            dbContext.Colors.Add(color);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var user = new UserEntity(color)
            {
                LoginId = "user2",
                PasswordHash = new string('c', 64),
                Salt = new string('d', 32),
                PrivilegeId = 0,
                ColorId = color.Id
            };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var originalCreatedAt = user.CreatedAt;
            var originalLastUpdatedAt = user.LastUpdatedAt;

            // Act
            user.Remark = "updated";
            await dbContext.SaveChangesAsync(CancellationToken.None);

            // Assert
            Assert.Equal(originalCreatedAt, user.CreatedAt);
            Assert.True(user.LastUpdatedAt > originalLastUpdatedAt);
        }

        /// <summary>
        /// 新規追加したColorEntity(UUIDなし)に対してCreatedAtとLastUpdatedAtが正しく設定されることを検証する
        /// </summary>
        [Fact]
        public async Task SaveChangesAsync_AddedColorEntity_SetsTimestamps()
        {
            var uuidGeneratorMock = new Mock<IUuidGenerator>();
            var dbContext = CreateDbContext(uuidGeneratorMock);

            var color = new ColorEntity { Name = "Green", Red = 0, Green = 255, Blue = 0 };
            dbContext.Colors.Add(color);

            await dbContext.SaveChangesAsync(CancellationToken.None);

            Assert.True(color.CreatedAt <= color.LastUpdatedAt);
        }

        /// <summary>
        /// 既存ColorEntity(UUIDなし)を更新した際、CreatedAtは変更されずLastUpdatedAtのみ更新されることを検証する
        /// </summary>
        [Fact]
        public async Task SaveChangesAsync_ModifiedColorEntity_UpdatesLastUpdatedAtOnly()
        {
            var uuidGeneratorMock = new Mock<IUuidGenerator>();
            var dbContext = CreateDbContext(uuidGeneratorMock);

            var color = new ColorEntity { Name = "Yellow", Red = 255, Green = 255, Blue = 0 };
            dbContext.Colors.Add(color);
            await dbContext.SaveChangesAsync(CancellationToken.None);

            var originalCreatedAt = color.CreatedAt;
            var originalLastUpdatedAt = color.LastUpdatedAt;

            color.Name = "LightYellow";
            await dbContext.SaveChangesAsync(CancellationToken.None);

            Assert.Equal(originalCreatedAt, color.CreatedAt);
            Assert.True(color.LastUpdatedAt > originalLastUpdatedAt);
        }
    }
}
