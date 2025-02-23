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

        /// <summary>
        /// 特に固有の初期化処理や特定の設定はないため、処理なしでbaseコンストラクタに渡す
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        /// <summary>
        /// 基本的に各エンティティ共通のマッピングや制約などを定義
        /// </summary>
        /// <remark>
        /// 各エンティティ個別で行う処理は各エンティティ個別のConfigureメソッドにて行うこと
        /// </remark>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // UUIDであるGuid型をchar(36)に変換する
            // 基本的にすべてのテーブルにUUIDがあるため、ここで一括で変換する
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    // Guid型またはNullable<Guid>型の場合に処理を適用
                    if (property.ClrType == typeof(Guid) || property.ClrType == typeof(Guid?))
                    {
                        property.SetColumnType("char(36)");
                    }
                }
            }

            // IEntityTypeConfigurationを継承するすべてのConfigureメソッドを実行する(各エンティティの設定)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
