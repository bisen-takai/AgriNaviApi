using AgriNaviApi.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AgriNaviApi.Infrastructure.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// カラー
        /// </summary>
        public DbSet<ColorEntity> Colors { get; set; }

        /// <summary>
        /// グループ
        /// </summary>
        public DbSet<GroupEntity> Groups { get; set; }

        /// <summary>
        /// ユーザ
        /// </summary>
        public DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// 圃場
        /// </summary>
        public DbSet<FieldEntity> Fields { get; set; }

        /// <summary>
        /// 作付
        /// </summary>
        public DbSet<CropEntity> Crops { get; set; }

        /// <summary>
        /// 品質規格
        /// </summary>
        public DbSet<QualityStandardEntity> QualityStandards { get; set; }

        /// <summary>
        /// 単位
        /// </summary>
        public DbSet<UnitEntity> Units { get; set; }

        /// <summary>
        /// 出荷先
        /// </summary>
        public DbSet<ShippingDestinationEntity> ShippingDestinations { get; set; }

        /// <summary>
        /// 年間作付計画
        /// </summary>
        public DbSet<SeasonCropScheduleEntity> SeasonCropSchedules { get; set; }

        /// <summary>
        /// 出荷記録
        /// </summary>
        public DbSet<ShipmentRecordEntity> ShipmentRecords { get; set; }

        /// <summary>
        /// 出荷記録詳細
        /// </summary>
        public DbSet<ShipmentRecordDetailEntity> ShipmentRecordDetails { get; set; }
    }
}
