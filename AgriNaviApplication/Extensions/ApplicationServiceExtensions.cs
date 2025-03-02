using AgriNaviApi.Application.Interfaces;
using AgriNaviApi.Application.Profiles;
using AgriNaviApi.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AgriNaviApi.Application.Extensions
{
    /// <summary>
    /// Application層のサービスを登録する拡張メソッド
    /// </summary>
    public static class ApplicationServiceExtensions
    {
        /// <summary>
        /// Application層のサービスを登録する
        /// </summary>
        /// <param name="services">builder.Services</param>
        /// <returns>builder.Services</returns>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IColorService, ColorService>();
            services.AddAutoMapper(typeof(ColorProfile).Assembly);

            services.AddScoped<IUnitService, UnitService>();
            services.AddAutoMapper(typeof(UnitProfile).Assembly);

            services.AddScoped<IQualityStandardService, QualityStandardService>();
            services.AddAutoMapper(typeof(QualityStandardProfile).Assembly);

            return services;
        }
    }
}
