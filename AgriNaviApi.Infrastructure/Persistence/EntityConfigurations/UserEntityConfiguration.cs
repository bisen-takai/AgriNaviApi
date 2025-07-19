using AgriNaviApi.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgriNaviApi.Infrastructure.Persistence.EntityConfigurations
{
    /// <summary>
    /// UserEntity のデータベースマッピング設定を行うクラス（Fluent API）
    /// </summary>
    public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        /// <summary>
        /// UserEntity に対するカラム型や制約の設定を定義する
        /// </summary>
        /// <param name="builder">エンティティのマッピングを構成するビルダー</param>
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(u => u.Uuid)
                   .HasColumnType("char(36)");
        }
    }
}
