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
    public static class ServiceConfiguration
    {
        public static IServiceCollection AddServicesFromInfrastructure(this IServiceCollection services, IConfiguration Configuration)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection").ToString();
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Lockout.AllowedForNewUsers = true;
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequiredLength = 8;
                })
                .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
            services.AddDbContext<AppDbContext>(options =>
                           options.UseSqlServer(connectionString)
                       );
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IPropertyTypeReposiotry, PropertyTypeRepository>();
            services.AddTransient<IPropertyReposiotry, PropertyRepository>();
            services.AddTransient<IPropertyImageReposiotry, PropertyImageRepository>();
            return services;
        }
    }
}
