using HouseBrokerMVP.Business.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerMVP.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServicesFromBusiness(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient<MigrationService>();
            services.AddTransient<IAuthService,AuthService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IPropertyTypeService, PropertyTypeService>();
            services.AddTransient<IPropertyService, PropertyService>();
            return services;
        }
    }
}
