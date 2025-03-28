using AgriNaviApi.Infrastructure.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Infrastructure.UnitTests
{
    public class SeasonCropScheduleEntityTests
    {
        /// <summary>
        /// コンストラクタテスト(ColorEntityがNull)
        /// </summary>
        [Fact]
        public void Constructor_NullColor()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var entity = new SeasonCropScheduleEntity(null!);
            });
        }

        /// <summary>
        /// コンストラクタテスト(正常)
        /// </summary>
        [Fact]
        public void Constructor_Normal()
        {
            var cropEntity = new CropEntity
            {
                Id = 1,
                Uuid = Guid.NewGuid(),
                Name = new string('a', 20),
                ShortName = "abcd",
                GroupId = 1,
                ColorId = 1,
                Remark = new string('a', 200),
                IsDeleted = true
            };

            var entity = new SeasonCropScheduleEntity(cropEntity);

            Assert.Equal(cropEntity, entity.Crop);
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
            var entity = new SeasonCropScheduleEntity();

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
            var entity = new SeasonCropScheduleEntity
            {
                Name = new string('あ', 30),
                Remark = new string('a', 200)
            };

            var errorResults = new List<ValidationResult>();
            var context = new ValidationContext(entity);
            bool isValid = Validator.TryValidateObject(entity, context, errorResults, true);

            Assert.True(isValid);
            Assert.Empty(errorResults);
        }

        /// <summary>
        /// 作付名文字数超過チェック
        /// </summary>
        [Theory]
        [InlineData("1234567890123456789012345678901")]   // 31文字
        [InlineData("ああああああああああああああああああああああああああああああ1")]  // 31文字
        public void Name_LongString_Abnormality(string validName)
        {
            var entity = new SeasonCropScheduleEntity
            {
                Name = validName, // 31文字にして StringLength(30) を超過
                CropId = 1,
                Remark = new string('a', 200),
                IsDeleted = true
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
            var entity = new SeasonCropScheduleEntity
            {
                Name = new string('a', 20),
                CropId = 1,
                Remark = new string('a', 201), // 201文字にして StringLength(200) を超過
                IsDeleted = true
            };

            var validationContext = new ValidationContext(entity);
            Assert.Throws<ValidationException>(() =>
                Validator.ValidateObject(entity, validationContext, validateAllProperties: true));
        }
    }
}
