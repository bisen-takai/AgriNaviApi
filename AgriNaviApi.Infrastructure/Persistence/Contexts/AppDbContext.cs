using AgriNaviApi.Shared.Utilities;
using AgriNaviApi.Infrastructure.Persistence.Entities;
using AgriNaviApi.Infrastructure.Persistence.Entities.Base;
using Microsoft.EntityFrameworkCore;
using AgriNaviApi.Shared.Interfaces;

namespace AgriNaviApi.Infrastructure.Persistence.Contexts
{
    public class AppDbContext : DbContext
    {
        private readonly IUuidGenerator _uuidGenerator;

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
        public DbSet<ShipDestinationEntity> ShipDestinations { get; set; }

        /// <summary>
        /// 年間作付計画
        /// </summary>
        public DbSet<SeasonScheduleEntity> SeasonSchedules { get; set; }

        /// <summary>
        /// 出荷記録
        /// </summary>
        public DbSet<ShipmentEntity> Shipments { get; set; }

        /// <summary>
        /// 出荷記録詳細
        /// </summary>
        public DbSet<ShipmentLineEntity> ShipmentLines { get; set; }

        /// <summary>
        /// 特に固有の初期化処理や特定の設定はないため、処理なしでbaseコンストラクタに渡す
        /// </summary>
        /// <param name="options"></param>
        public AppDbContext(DbContextOptions<AppDbContext> options, IUuidGenerator uuidGenerator) : base(options)
        {
            _uuidGenerator = uuidGenerator;
        }

        /// <summary>
        /// モデル作成時にエンティティ構成を適用する（Fluent API）
        /// </summary>
        /// <remark>
        /// 各エンティティ個別で行う処理は各エンティティ個別のConfigureメソッドにて行うこと
        /// </remark>
        /// <param name="modelBuilder">エンティティモデル構築用のビルダー</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // IEntityTypeConfigurationを継承するすべてのConfigureメソッドを実行する(各エンティティの設定)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        /// <summary>
        /// 非同期で変更をデータベースに保存する際に、共通プロパティ(UUID・登録日時・更新日時 を自動生成する
        /// </summary>
        /// <param name="cancellationToken">キャンセル要求のトークン</param>
        /// <remarks>
        /// 以下の自動設定を行う：
        /// ・IHasUuidを実装していてUUIDが空(Guid.Empty)の場合、新しいUUIDを生成して設定する。<br/>
        /// ・BaseEntityを継承している場合、登録時は CreatedAt と LastUpdatedAt を設定し、更新時は LastUpdatedAt のみを更新する。
        /// </remarks>
        /// <returns>保存されたエンティティ数</returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            // UUIDの自動生成
            var entriesToSetUuid = ChangeTracker.Entries<IHasUuid>()
                .Where(e => e.State == EntityState.Added &&
                            e.Property(nameof(IHasUuid.Uuid)).CurrentValue is Guid guid && guid == Guid.Empty);

            foreach (var entry in entriesToSetUuid)
            {
                entry.Property(nameof(IHasUuid.Uuid)).CurrentValue = _uuidGenerator.GenerateUuid();
            }

            // タイムスタンプの自動設定
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(BaseEntity.CreatedAt)).CurrentValue = DateTime.UtcNow;
                    entry.Property(nameof(BaseEntity.LastUpdatedAt)).CurrentValue = DateTime.UtcNow;

                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(BaseEntity.LastUpdatedAt)).CurrentValue = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
