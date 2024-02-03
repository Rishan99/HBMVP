using HouseBrokerMVP.Core.Context;
using HouseBrokerMVP.Core.Repositories;
using HouseBrokerMVP.Infrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using HouseBrokerMVP.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace HouseBrokerMVP.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServicesFromInfrastructure(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>().AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
            var connectionString = Configuration.GetConnectionString("DefaultConnection").ToString();
            services.AddDbContext<AppDbContext>(options =>
                           options.UseSqlServer(connectionString, b => b.MigrationsAssembly("HouseBrokerMVP.API"))
                       );
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IPropertyTypeReposiotry, PropertyTypeRepository>();
            services.AddTransient<IPropertyReposiotry, PropertyRepository>();
            services.AddTransient<IPropertyImageReposiotry, PropertyImageRepository>();
            return services;
        }
    }
}
