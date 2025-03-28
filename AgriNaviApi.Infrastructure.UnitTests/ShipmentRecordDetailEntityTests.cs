using AgriNaviApi.Infrastructure.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace AgriNaviApi.Infrastructure.UnitTests
{
    public class ShipmentRecordDetailEntityTests
    {
        /// <summary>
        /// コンストラクタテスト(ShipmentRecordEntityがNull)
        /// </summary>
        [Fact]
        public void Constructor_NullShipmentRecord()
        {
            var shipmentRecord = new ShipmentRecordEntity { Id = 1, Uuid = Guid.NewGuid(), RecordDate = DateTime.Now, FieldId = 1, CropId = 1, SeasonCropScheduleId = 1, Remark = "ab", IsDeleted = true };
            var shippingDestination = new ShippingDestinationEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "ShippingDestination", IsDeleted = true };
            var qualityStandard = new QualityStandardEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "QualityStandard", IsDeleted = true };
            var unit = new UnitEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "Unit", IsDeleted = true };

            Assert.Throws<ArgumentNullException>(() =>
            {
                var entity = new ShipmentRecordDetailEntity(null!, shippingDestination, qualityStandard, unit);
            });
        }

        /// <summary>
        /// コンストラクタテスト(ShippingDestinationEntityがNull)
        /// </summary>
        [Fact]
        public void Constructor_NullShippingDestination()
        {
            var shipmentRecord = new ShipmentRecordEntity { Id = 1, Uuid = Guid.NewGuid(), RecordDate = DateTime.Now, FieldId = 1, CropId = 1, SeasonCropScheduleId = 1, Remark = "ab", IsDeleted = true };
            var shippingDestination = new ShippingDestinationEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "ShippingDestination", IsDeleted = true };
            var qualityStandard = new QualityStandardEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "QualityStandard", IsDeleted = true };
            var unit = new UnitEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "Unit", IsDeleted = true };

            Assert.Throws<ArgumentNullException>(() =>
            {
                var entity = new ShipmentRecordDetailEntity(shipmentRecord, null!, qualityStandard, unit);
            });
        }

        /// <summary>
        /// コンストラクタテスト(QualityStandardEntityがNull)
        /// </summary>
        [Fact]
        public void Constructor_NullQualityStandard()
        {
            var shipmentRecord = new ShipmentRecordEntity { Id = 1, Uuid = Guid.NewGuid(), RecordDate = DateTime.Now, FieldId = 1, CropId = 1, SeasonCropScheduleId = 1, Remark = "ab", IsDeleted = true };
            var shippingDestination = new ShippingDestinationEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "ShippingDestination", IsDeleted = true };
            var qualityStandard = new QualityStandardEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "QualityStandard", IsDeleted = true };
            var unit = new UnitEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "Unit", IsDeleted = true };

            Assert.Throws<ArgumentNullException>(() =>
            {
                var entity = new ShipmentRecordDetailEntity(shipmentRecord, shippingDestination, null!, unit);
            });
        }

        /// <summary>
        /// コンストラクタテスト(UnitEntityがNull)
        /// </summary>
        [Fact]
        public void Constructor_NullUnit()
        {
            var shipmentRecord = new ShipmentRecordEntity { Id = 1, Uuid = Guid.NewGuid(), RecordDate = DateTime.Now, FieldId = 1, CropId = 1, SeasonCropScheduleId = 1, Remark = "ab", IsDeleted = true };
            var shippingDestination = new ShippingDestinationEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "ShippingDestination", IsDeleted = true };
            var qualityStandard = new QualityStandardEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "QualityStandard", IsDeleted = true };
            var unit = new UnitEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "Unit", IsDeleted = true };

            Assert.Throws<ArgumentNullException>(() =>
            {
                var entity = new ShipmentRecordDetailEntity(shipmentRecord, shippingDestination, qualityStandard, null!);
            });
        }

        /// <summary>
        /// コンストラクタテスト(正常)
        /// </summary>
        [Fact]
        public void Constructor_Normal()
        {
            var shipmentRecord = new ShipmentRecordEntity { Id = 1, Uuid = Guid.NewGuid(), RecordDate = DateTime.Now, FieldId = 1, CropId = 1, SeasonCropScheduleId = 1, Remark = "ab", IsDeleted = true };
            var shippingDestination = new ShippingDestinationEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "ShippingDestination", IsDeleted = true };
            var qualityStandard = new QualityStandardEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "QualityStandard", IsDeleted = true };
            var unit = new UnitEntity { Id = 1, Uuid = Guid.NewGuid(), Name = "Unit", IsDeleted = true };

            var entity = new ShipmentRecordDetailEntity(shipmentRecord, shippingDestination, qualityStandard, unit);

            Assert.Equal(shipmentRecord, entity.ShipmentRecord);
            Assert.Equal(shippingDestination, entity.ShippingDestination);
            Assert.Equal(qualityStandard, entity.QualityStandard);
            Assert.Equal(unit, entity.Unit); 
            Assert.NotEqual(default, entity.CreatedAt);
            Assert.NotEqual(default, entity.LastUpdatedAt);
        }

        /// <summary>
        /// 初期化時のデフォルト値の確認
        /// </summary>
        [Fact]
        public void DefaultConstructor_InitializesProperties()
        {
            var entity = new ShipmentRecordDetailEntity();

            Assert.NotEqual(default, entity.CreatedAt);
            Assert.NotEqual(default, entity.LastUpdatedAt);
        }

        /// <summary>
        /// 境界値の正常確認テスト
        /// </summary>
        [Fact]
        public void Quantity_Range_Normal()
        {
            var entity = new ShipmentRecordDetailEntity
            {
                ShipmentRecordId = 1,
                ShippingDestinationId = 1,
                QualityStandardId = 1,
                Quantity = 100000,
                UnitId = 1,
                Amount = 1000000
            };

            var errorResults = new List<ValidationResult>();
            var context = new ValidationContext(entity);
            bool isValid = Validator.TryValidateObject(entity, context, errorResults, true);

            Assert.True(isValid);
            Assert.Empty(errorResults);
        }

        /// <summary>
        /// 数量超過チェック
        /// </summary>
        [Fact]
        public void Quantity_Range_Abnormality()
        {
            var entity = new ShipmentRecordDetailEntity
            {
                ShipmentRecordId = 1,
                ShippingDestinationId = 1,
                QualityStandardId = 1,
                Quantity = 100001,
                UnitId = 1,
                Amount = 1000000
            };

            var validationContext = new ValidationContext(entity);
            Assert.Throws<ValidationException>(() =>
                Validator.ValidateObject(entity, validationContext, validateAllProperties: true));
        }

        /// <summary>
        /// 金額超過チェック
        /// </summary>
        [Fact]
        public void Amount_Range_Abnormality()
        {
            var entity = new ShipmentRecordDetailEntity
            {
                ShipmentRecordId = 1,
                ShippingDestinationId = 1,
                QualityStandardId = 1,
                Quantity = 100000,
                UnitId = 1,
                Amount = 1000001
            };

            var validationContext = new ValidationContext(entity);
            Assert.Throws<ValidationException>(() =>
                Validator.ValidateObject(entity, validationContext, validateAllProperties: true));
        }
    }
}
