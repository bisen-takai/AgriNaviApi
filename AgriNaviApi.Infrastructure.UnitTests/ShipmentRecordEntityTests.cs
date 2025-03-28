using AgriNaviApi.Infrastructure.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Infrastructure.UnitTests
{
    public class ShipmentRecordEntityTests
    {
        /// <summary>
        /// コンストラクタテスト(FieldEntityがNull)
        /// </summary>
        [Fact]
        public void Constructor_NullField()
        {
            FieldEntity fieldEntity = new FieldEntity();
            CropEntity cropEntity = new CropEntity();
            SeasonCropScheduleEntity seasonCropScheduleEntity = new SeasonCropScheduleEntity();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var entity = new ShipmentRecordEntity(null!, cropEntity, seasonCropScheduleEntity);
            });
        }

        /// <summary>
        /// コンストラクタテスト(CropEntityがNull)
        /// </summary>
        [Fact]
        public void Constructor_NullCrop()
        {
            FieldEntity fieldEntity = new FieldEntity();
            CropEntity cropEntity = new CropEntity();
            SeasonCropScheduleEntity seasonCropScheduleEntity = new SeasonCropScheduleEntity();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var entity = new ShipmentRecordEntity(fieldEntity, null!, seasonCropScheduleEntity);
            });
        }

        /// <summary>
        /// コンストラクタテスト(SeasonCropScheduleEntityがNull)
        /// </summary>
        [Fact]
        public void Constructor_NullSeasonCropSchedule()
        {
            FieldEntity fieldEntity = new FieldEntity();
            CropEntity cropEntity = new CropEntity();
            SeasonCropScheduleEntity seasonCropScheduleEntity = new SeasonCropScheduleEntity();

            Assert.Throws<ArgumentNullException>(() =>
            {
                var entity = new ShipmentRecordEntity(fieldEntity, cropEntity, null!);
            });
        }

        /// <summary>
        /// コンストラクタテスト(正常)
        /// </summary>
        [Fact]
        public void Constructor_Normal()
        {
            FieldEntity fieldEntity = new FieldEntity { Name = new string('a', 20), ShortName = "abcd", FieldSize = 100000, GroupId = 1, ColorId = 2, Remark = new string('a', 200) };
            CropEntity cropEntity = new CropEntity { Name = new string('a', 20), ShortName = "abcd", GroupId = 1, ColorId = 2, Remark = new string('b', 200) };
            SeasonCropScheduleEntity seasonCropScheduleEntity = new SeasonCropScheduleEntity { Name = new string('あ', 30), CropId = 1, Remark = new string('a', 200), IsDeleted = true };

            var entity = new ShipmentRecordEntity(fieldEntity, cropEntity, seasonCropScheduleEntity);

            Assert.Equal(fieldEntity, entity.Field);
            Assert.Equal(cropEntity, entity.Crop);
            Assert.Equal(seasonCropScheduleEntity, entity.SeasonCropSchedule);
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
            FieldEntity fieldEntity = new FieldEntity { Name = new string('a', 20), ShortName = "abcd", FieldSize = 100000, GroupId = 1, ColorId = 2, Remark = new string('a', 200) };
            CropEntity cropEntity = new CropEntity { Name = new string('a', 20), ShortName = "abcd", GroupId = 1, ColorId = 2, Remark = new string('b', 200) };
            SeasonCropScheduleEntity seasonCropScheduleEntity = new SeasonCropScheduleEntity { Name = new string('あ', 30), CropId = 1, Remark = new string('a', 200), IsDeleted = true };

            var entity = new ShipmentRecordEntity(fieldEntity, cropEntity, seasonCropScheduleEntity);

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
            var entity = new ShipmentRecordEntity
            {
                Remark = new string('あ', 200)
            };

            var errorResults = new List<ValidationResult>();
            var context = new ValidationContext(entity);
            bool isValid = Validator.TryValidateObject(entity, context, errorResults, true);

            Assert.True(isValid);
            Assert.Empty(errorResults);
        }

        /// <summary>
        /// 備考文字数超過チェック
        /// </summary>
        [Fact]
        public void Remark_LongString_Abnormality()
        {
            var entity = new ShipmentRecordEntity
            {
                Remark = new string('a', 201) // 201文字にして StringLength(200) を超過
            };

            var validationContext = new ValidationContext(entity);
            Assert.Throws<ValidationException>(() =>
                Validator.ValidateObject(entity, validationContext, validateAllProperties: true));
        }
    }
}
