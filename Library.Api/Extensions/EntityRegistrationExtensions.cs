using Library.Api.Services;
using Library.Api.Utilities;
using Library.Domain.DBQueries;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Api.Extensions
{
    public static class EntityRegistrationExtensions
    {

        public static void AddEntityServices(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.AddTransient<IAuthorService, AuthorServices>();
            services.AddTransient<IBookService, BookServices>();
            services.AddTransient(typeof(AuthorQueries));
            services.AddTransient<IAuthService, AuthServices>();
            services.AddTransient<IRentalService, RentalServices>();
            services.AddTransient<IResourceUtil, ResourceUtil>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

        }
    }
}
