using AgriNaviApi.Application.Common;
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
            services.AddScoped<IQualityStandardService, QualityStandardService>();
            services.AddScoped<IShipDestinationService, ShipDestinationService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IFieldService, FieldService>();
            services.AddScoped<ICropService, CropService>();
            services.AddScoped<ISeasonScheduleService, SeasonScheduleService>();
            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<IShipmentLineService, ShipmentLineService>();
            services.AddScoped<IShipmentWithLineService, ShipmentWithLineService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<ForeignKeyValidator>();

            services.AddAutoMapper(
                typeof(ColorProfile),
                typeof(QualityStandardProfile),
                typeof(ShipDestinationProfile),
                typeof(UnitProfile),
                typeof(GroupProfile),
                typeof(FieldProfile),
                typeof(CropProfile),
                typeof(SeasonScheduleProfile),
                typeof(ShipmentProfile),
                typeof(ShipmentLineProfile),
                typeof(ShipmentWithLineProfile),
                typeof(UserProfile)
            );

            return services;
        }
    }
}
