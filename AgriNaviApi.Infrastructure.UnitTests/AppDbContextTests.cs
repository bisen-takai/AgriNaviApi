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
        /// Guid�^��char(36)�ɕϊ��ł��邱�Ƃ��m�F
        /// </summary>
        [Fact]
        public void GuidProperty_IsConfiguredAsChar36()
        {
            using var context = new AppDbContext(_options);
            var model = context.Model;

            // ColorEntity�̃G���e�B�e�B�^���擾
            var entityType = model.FindEntityType(typeof(ColorEntity));
            Assert.NotNull(entityType);

            // ColorEntity��Uuid�v���p�e�B���擾
            var guidProperty = entityType.FindProperty(nameof(ColorEntity.Uuid));
            Assert.NotNull(guidProperty);

            // �ݒ肳��Ă���J�����^�� "char(36)" �ɂȂ��Ă��邱�Ƃ��m�F
            var columnType = guidProperty.GetColumnType();
            Assert.Equal("char(36)", columnType);
        }
    }
}