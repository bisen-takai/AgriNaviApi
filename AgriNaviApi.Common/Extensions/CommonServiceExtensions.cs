using AgriNaviApi.Common.Interfaces;
using AgriNaviApi.Common.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace AgriNaviApi.Common.Extensions
{
    /// <summary>
    /// Common層のサービスを登録する拡張メソッド
    /// </summary>
    public static class CommonServiceExtensions
    {
        /// <summary>
        /// Common層のサービスを登録する
        /// </summary>
        /// <param name="services">builder.Services</param>
        /// <returns>builder.Services</returns>
        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            services.AddScoped<IUuidGenerator, UuidGenerator>();
            services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();
            services.AddScoped<ISaltGenerator, SaltGenerator>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
