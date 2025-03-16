using AgriNaviApi.Common.Enums;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Infrastructure.UnitTests
{
    public class CropEntityTests
    {
        /// <summary>
        /// コンストラクタテスト(ColorEntityがNull)
        /// </summary>
        [Fact]
        public void Constructor_NullColor()
        {
            GroupEntity groupEntity = new GroupEntity();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var entity = new CropEntity(groupEntity, null!);
            });
        }

        /// <summary>
        /// コンストラクタテスト(GroupEntityがNull)
        /// </summary>
        [Fact]
        public void Constructor_NullGroup()
        {
            ColorEntity colorEntity = new ColorEntity();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var entity = new CropEntity(null!, colorEntity);
            });
        }

        /// <summary>
        /// コンストラクタテスト(正常)
        /// </summary>
        [Fact]
        public void Constructor_Normal()
        {
            var group = new GroupEntity { Id = 1, Uuid = Guid.NewGuid(), Kind = GroupKind.Farm, Name = "GROUP", IsDeleted = true };
            var color = new ColorEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "COLOR", RedValue = 0, GreenValue = 125, BlueValue = 255 };

            var entity = new CropEntity(group, color);

            Assert.Equal(group, entity.Group);
            Assert.Equal(color, entity.Color);
            Assert.False(entity.IsDeleted);
            Assert.NotEqual(default, entity.CreatedAt);
            Assert.NotEqual(default, entity.LastUpdatedAt);
        }

        /// <summary>
        /// 初期化時のデフォルト値の確認
        /// </summary>
        [Fact]
        public void DefaultConstructor_InitializesProperties()
        {
            var entity = new CropEntity();

            Assert.Equal(string.Empty, entity.Name);
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
            var entity = new CropEntity
            {
                Name = new string('a', 20),
                ShortName = "abcd",
                GroupId = 1,
                ColorId = 1,
                Remark = new string('a', 200)
            };

            Assert.Equal(new string('a', 20), entity.Name);
            Assert.Equal("abcd", entity.ShortName);
            Assert.Equal(1, entity.GroupId);
            Assert.Equal(1, entity.ColorId);
            Assert.Equal(new string('a', 200), entity.Remark);
        }

        /// <summary>
        /// 作付名文字数超過チェック
        /// </summary>
        [Theory]
        [InlineData("123456789012345678901")]   // 21文字
        [InlineData("ああああああああああああああああああああ1")]  // 21文字
        public void Name_LongString_Abnormality(string validName)
        {
            var entity = new CropEntity
            {
                Name = validName, // 21文字にして StringLength(20) を超過
                ShortName = "abcd",
                GroupId = 1,
                ColorId = 1,
                Remark = new string('a', 200)
            };

            var validationContext = new ValidationContext(entity);
            Assert.Throws<ValidationException>(() =>
                Validator.ValidateObject(entity, validationContext, validateAllProperties: true));
        }

        /// <summary>
        /// 作付名省略名文字数超過チェック
        /// </summary>
        [Theory]
        [InlineData("12345")]   // 5文字
        [InlineData("あああああ")]  // 5文字
        public void ShortName_LongString_Abnormality(string validName)
        {
            var entity = new CropEntity
            {
                Name = new string('a', 20),
                ShortName = validName, // 5文字にして StringLength(4) を超過
                GroupId = 1,
                ColorId = 1,
                Remark = new string('a', 200)
            };

            var validationContext = new ValidationContext(entity);
            Assert.Throws<ValidationException>(() =>
                Validator.ValidateObject(entity, validationContext, validateAllProperties: true));
        }

        /// <summary>
        /// 備考文字数超過チェック
        /// </summary>
        [Fact]
        public void Remark_LongString_Abnormality()
        {
            var entity = new CropEntity
            {
                Name = new string('a', 20),
                ShortName = "abcd",
                GroupId = 1,
                ColorId = 1,
                Remark = new string('a', 201) // 201文字にして StringLength(200) を超過
            };

            var validationContext = new ValidationContext(entity);
            Assert.Throws<ValidationException>(() =>
                Validator.ValidateObject(entity, validationContext, validateAllProperties: true));
        }
    }
}
