using AgriNaviApi.Shared.Interfaces;
using AgriNaviApi.Shared.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace AgriNaviApi.Shared.Extensions
{
    public static class SharedServiceExtensions
    {
        /// <summary>
        /// Shared層のサービスを登録する
        /// </summary>
        /// <param name="services">依存性注入用のIServiceCollection</param>
        /// <returns>依存性注入用のIServiceCollection</returns>
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {
            services.AddScoped<ISaltGenerator, SaltGenerator>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IUuidGenerator, UuidGenerator>();

            return services;
        }
    }
}
