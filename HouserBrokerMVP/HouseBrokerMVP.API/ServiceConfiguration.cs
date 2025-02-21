using HouseBrokerMVP.Core.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace HouseBrokerMVP.API
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection ConfigServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Swagger
            services.AddSwaggerGen(s =>
            {
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "LOGIN",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                    });
            });

            services.AddEndpointsApiExplorer();

            var jwtConfig = configuration.GetSection("JwtConfig").Get<JwtConfig>()!;
            services.AddSingleton((x) => jwtConfig);
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Policy for CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllAllowPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            //JWT Related
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.RequireAuthenticatedSignIn = true;
            })
             .AddJwtBearer(cfg =>
             {
                 cfg.RequireHttpsMetadata = false;
                 cfg.SaveToken = true;
                 cfg.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidIssuer = jwtConfig.Issuer,
                     ValidateIssuer = true,
                     ValidateLifetime = true,
                     ValidAudience = jwtConfig.Issuer,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key)),
                     ClockSkew = TimeSpan.Zero,
                 };
             });
            return services;
        }
    }
}
