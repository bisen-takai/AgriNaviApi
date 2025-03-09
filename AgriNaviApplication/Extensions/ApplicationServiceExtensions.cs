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

            services.AddScoped<IGroupService, GroupService>();
            services.AddAutoMapper(typeof(GroupProfile).Assembly);

            services.AddScoped<ICropService, CropService>();
            services.AddAutoMapper(typeof(CropProfile).Assembly);

            services.AddScoped<IFieldService, FieldService>();
            services.AddAutoMapper(typeof(FieldProfile).Assembly);

            services.AddScoped<IShippingDestinationService, ShippingDestinationService>();
            services.AddAutoMapper(typeof(ShippingDestinationProfile).Assembly);

            services.AddScoped<ISeasonCropScheduleService, SeasonCropScheduleService>();
            services.AddAutoMapper(typeof(SeasonCropScheduleProfile).Assembly);

            services.AddScoped<IUserService, UserService>();
            services.AddAutoMapper(typeof(UserProfile).Assembly);

            services.AddScoped<IShipmentRecordService, ShipmentRecordService>();
            services.AddAutoMapper(typeof(ShipmentRecordProfile).Assembly);

            return services;
        }
    }
}
