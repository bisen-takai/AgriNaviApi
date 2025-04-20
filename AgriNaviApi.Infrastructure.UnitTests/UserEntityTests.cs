using AgriNaviApi.Common.Enums;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AgriNaviApi.Infrastructure.UnitTests
{
    public class UserEntityTests
    {
        /// <summary>
        /// コンストラクタテスト(ColorEntityがNull)
        /// </summary>
        [Fact]
        public void Constructor_NullColor()
        {
            ColorEntity colorEntity = new ColorEntity();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var entity = new UserEntity(colorEntity);
            });
        }

        /// <summary>
        /// コンストラクタテスト(正常)
        /// </summary>
        [Fact]
        public void Constructor_Normal()
        {
            var color = new ColorEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "COLOR", RedValue = 0, GreenValue = 125, BlueValue = 255 };

            var entity = new UserEntity(color);

            Assert.Equal(color, entity.Color);
        }

        /// <summary>
        /// 初期化時のデフォルト値の確認
        /// </summary>
        [Fact]
        public void DefaultConstructor_InitializesProperties()
        {
            var entity = new UserEntity();

            Assert.Equal(string.Empty, entity.LoginId);
            Assert.Equal(string.Empty, entity.PasswordHash);
            Assert.Equal(string.Empty, entity.Salt);
            Assert.False(entity.IsDeleted);
            Assert.NotEqual(default, entity.CreatedAt);
            Assert.NotEqual(default, entity.LastUpdatedAt);
        }

        /// <summary>
        /// 境界値の正常確認テスト
        /// </summary>
        [Fact]
        public void BoundaryValue_Normal()
        {
            var entity = new UserEntity
            {
                LoginId = new string('a', 20),
                PasswordHash = new string('a', 64),
                Salt = new string('a', 24),
                FullName = new string('a', 20),
                PhoneNumber = new string('a', 10),
                Address = new string('a', 30),
                Remark = new string('a', 200),
                PrivilegeId = PrivilegeKind.Guest
            };

            var errorResults = new List<ValidationResult>();
            var context = new ValidationContext(entity);
            bool isValid = Validator.TryValidateObject(entity, context, errorResults, true);

            Assert.True(isValid);
            Assert.Empty(errorResults);
        }
    }
}
