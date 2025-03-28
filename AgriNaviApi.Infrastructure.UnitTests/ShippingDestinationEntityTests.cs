using AgriNaviApi.Infrastructure.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Infrastructure.UnitTests
{
    public class ShippingDestinationEntityTests
    {
        /// <summary>
        /// 初期化時のデフォルト値の確認
        /// </summary>
        [Fact]
        public void DefaultConstructor_InitializesProperties()
        {
            var entity = new ShippingDestinationEntity();

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
            var entity = new ShippingDestinationEntity
            {
                Name = new string('a', 20)
            };

            var errorResults = new List<ValidationResult>();
            var context = new ValidationContext(entity);
            bool isValid = Validator.TryValidateObject(entity, context, errorResults, true);

            Assert.True(isValid);
            Assert.Empty(errorResults);
        }

        /// <summary>
        /// 出荷先名文字数超過チェック
        /// </summary>
        [Theory]
        [InlineData("123456789012345678901")]   // 21文字
        [InlineData("ああああああああああああああああああああ1")]  // 21文字
        public void Name_LongString_Abnormality(string validName)
        {
            var entity = new ShippingDestinationEntity
            {
                Name = validName
            };

            var validationContext = new ValidationContext(entity);
            Assert.Throws<ValidationException>(() =>
                Validator.ValidateObject(entity, validationContext, validateAllProperties: true));
        }
    }
}
