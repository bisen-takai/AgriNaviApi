using AgriNaviApi.Common.Enums;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgriNaviApi.Infrastructure.UnitTests
{
    public class GroupEntityTests
    {
        /// <summary>
        /// 初期化時のデフォルト値の確認
        /// </summary>
        [Fact]
        public void DefaultConstructor_InitializesProperties()
        {
            var entity = new GroupEntity();

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
            var entity = new GroupEntity
            {
                Name = new string('a', 20),
                Kind = GroupKind.Other,
            };

            Assert.Equal(new string('a', 20), entity.Name);
            Assert.Equal(GroupKind.Other, entity.Kind);
        }

        /// <summary>
        /// グループ名文字数超過チェック
        /// </summary>
        [Theory]
        [InlineData("123456789012345678901")]   // 21文字
        [InlineData("ああああああああああああああああああああ1")]  // 21文字
        public void Name_LongString_Abnormality(string validName)
        {
            var entity = new GroupEntity
            {
                Name = validName,
                Kind = GroupKind.Other
            };

            var validationContext = new ValidationContext(entity);
            Assert.Throws<ValidationException>(() =>
                Validator.ValidateObject(entity, validationContext, validateAllProperties: true));
        }

        /// <summary>
        /// グループ種別範囲チェック
        /// </summary>
        [Theory]
        [InlineData(GroupKind.Undefined, true)]  // 0: 有効
        [InlineData(GroupKind.Farm, true)]  // 1: 有効
        [InlineData(GroupKind.Planting, true)]  // 2: 有効
        [InlineData(GroupKind.Other, true)]  // 3: 有効
        [InlineData((GroupKind)4, false)]    // 4: 無効
        public void GroupKind_Range_Abnormality(GroupKind kind, bool isValid)
        {
            var entity = new GroupEntity
            {
                Name = new string('a', 20),
                Kind = kind
            };

            var results = ValidateModel(entity);

            if (isValid)
            {
                Assert.Empty(results);  // エラーがないことを確認
            }
            else
            {
                Assert.NotEmpty(results);  // バリデーションエラーがあることを確認
            }
        }


        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>(); // バリデーションエラーを格納するリスト
            var validationContext = new ValidationContext(model, null, null); // 検証対象のコンテキスト
            Validator.TryValidateObject(model, validationContext, validationResults, true); // 実際にバリデーションを実行
            return validationResults; // 結果を返す
        }
    }
}
