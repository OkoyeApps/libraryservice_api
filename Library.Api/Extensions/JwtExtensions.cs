using Library.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Domain.Interfaces;
using Library.Api.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Library.Api.Middlewares;

namespace Library.Api.Extensions
{
    public static class JwtExtensions
    {
        private static readonly string SectionName = "JwtOptions";

        public static void AddJwt(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var section = configuration.GetSection(SectionName);
            var options = configuration.GetOptions<JwtOptions>(SectionName);
            services.Configure<JwtOptions>(section);
            services.AddSingleton(options);
            services.AddSingleton<IJwtHandler, JwtServices>();
            services.AddTransient<IAccessTokenService, AccessTokenServices>();
            services.AddTransient<AccessTokenValidationMiddleware>();
            services.AddAuthentication("OAuth")
                 .AddJwtBearer("OAuth",config =>
                 {
                     config.TokenValidationParameters = new TokenValidationParameters
                     {
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey)),
                         ValidIssuer = options.Issuer,
                         ValidAudience = options.ValidAudience,
                         ValidateAudience = options.ValidateAudience,
                         ValidateLifetime = options.ValidateLifetime,
                         ClockSkew = TimeSpan.Zero
                     };

                 });
        }


    }
}
