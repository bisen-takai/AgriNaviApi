using AgriNaviApi.Infrastructure.Persistence.Contexts;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgriNaviApi.Infrastructure.UnitTests
{
    public class AppDbContextTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public AppDbContextTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseMySql("Server = localhost; Database = agri_test; User = agri; Password = password;", new MySqlServerVersion(new Version(8, 4, 3)))
                .Options;
        }

        /// <summary>
        /// Guid型をchar(36)に変換できることを確認
        /// </summary>
        [Fact]
        public void GuidProperty_IsConfiguredAsChar36()
        {
            using var context = new AppDbContext(_options);
            var model = context.Model;

            // ColorEntityのエンティティ型を取得
            var entityType = model.FindEntityType(typeof(ColorEntity));
            Assert.NotNull(entityType);

            // ColorEntityのUuidプロパティを取得
            var guidProperty = entityType.FindProperty(nameof(ColorEntity.Uuid));
            Assert.NotNull(guidProperty);

            // 設定されているカラム型が "char(36)" になっていることを確認
            var columnType = guidProperty.GetColumnType();
            Assert.Equal("char(36)", columnType);
        }
    }
}