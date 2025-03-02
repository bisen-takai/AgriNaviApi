using AgriNaviApi.Infrastructure.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Infrastructure.UnitTests
{
    public class ColorEntityTests
    {
        /// <summary>
        /// ColorEntityの正常系の標準テストケースを確認
        /// </summary>
        [Fact]
        public void ColorEntity_Validation_Succeed_Normal()
        {
            var color = new ColorEntity
            {
                Uuid = Guid.NewGuid(),
                Name = "Red",
                RedValue = 255,
                GreenValue = 125,
                BlueValue = 0,
            };

            var errorResults = new List<ValidationResult>();
            var context = new ValidationContext(color);
            bool isValid = Validator.TryValidateObject(color, context, errorResults, true);

            Assert.True(isValid);
            Assert.Empty(errorResults);
        }

        /// <summary>
        /// カラー名の境界値の正常確認テスト
        /// </summary>
        /// <param name="validName"></param>
        [Theory]
        [InlineData("12345678901234567890")]  // 20文字（境界値）
        [InlineData("1234567890123456789")]   // 19文字
        [InlineData("ああああああああああああああああああああ")]  // 20文字（境界値）
        [InlineData("いいいいいいいいいいいいいいいいいいい")]   // 19文字
        public void ColorName_BoundaryValue_Normal(string validName)
        {
            var color = new ColorEntity
            {
                Uuid = Guid.NewGuid(),
                Name = validName,
                RedValue = 100,
                GreenValue = 100,
                BlueValue = 100
            };

            var errorResults = new List<ValidationResult>();
            var context = new ValidationContext(color);
            bool isValid = Validator.TryValidateObject(color, context, errorResults, true);

            Assert.True(isValid);
            Assert.Empty(errorResults);
        }

        /// <summary>
        /// カラー名の境界値の異常確認テスト
        /// </summary>
        /// <param name="validName"></param>
        [Theory]
        [InlineData("123456789012345678901")]   // 21文字
        [InlineData("ああああああああああああああああああああ1")]  // 21文字
        public void ColorName_BoundaryValue_Abnormality(string validName)
        {
            var color = new ColorEntity
            {
                Uuid = Guid.NewGuid(),
                Name = validName,
                RedValue = 100,
                GreenValue = 100,
                BlueValue = 100
            };

            var errorResults = new List<ValidationResult>();
            var context = new ValidationContext(color);
            bool isValid = Validator.TryValidateObject(color, context, errorResults, true);

            Assert.False(isValid);
            // 異常時のプロパティ名に"Name" が含まれていれば、21文字の場合は異常となったと判断
            Assert.Contains(errorResults, r => r.MemberNames.Contains("Name"));
        }

        /// <summary>
        /// 各RGB値が0-255の同値、境界値の正常確認テスト
        /// </summary>
        /// <param name="value"></param>
        [Theory]
        [InlineData(0)]
        [InlineData(128)]
        [InlineData(255)]
        public void RGB_Range_Normal(int value)
        {
            // Arrange
            var color = new ColorEntity
            {
                Uuid = Guid.NewGuid(),
                Name = "TestColor",
                RedValue = value,
                GreenValue = value,
                BlueValue = value
            };

            var errorResults = new List<ValidationResult>();
            var context = new ValidationContext(color);
            bool isValid = Validator.TryValidateObject(color, context, errorResults, true);

            Assert.True(isValid);
            Assert.Empty(errorResults);
        }

        /// <summary>
        /// 各RGB値が0-255の境界値の異常確認テスト
        /// </summary>
        /// <param name="value"></param>
        [Theory]
        [InlineData(-1)]
        [InlineData(256)]
        public void RGB_Range_Abnormality(int value)
        {
            // Arrange
            var color = new ColorEntity
            {
                Uuid = Guid.NewGuid(),
                Name = "TestColor",
                RedValue = value,
                GreenValue = value,
                BlueValue = value
            };

            var errorResults = new List<ValidationResult>();
            var context = new ValidationContext(color);
            bool isValid = Validator.TryValidateObject(color, context, errorResults, true);

            Assert.False(isValid);
            // 異常時のプロパティ名に各RGBが含まれていれば、異常となったと判断
            Assert.Contains(errorResults, r => r.MemberNames.Contains("RedValue"));
            Assert.Contains(errorResults, r => r.MemberNames.Contains("GreenValue"));
            Assert.Contains(errorResults, r => r.MemberNames.Contains("BlueValue"));
        }
    }
}
