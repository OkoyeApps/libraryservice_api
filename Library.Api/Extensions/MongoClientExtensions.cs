using Library.Domain.DbContext;
using Library.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Api.Extensions
{
    public static class MongoClientExtensions
    {
        private static readonly string SectionName = "DefaultConnection";

        public static void AddMongoConnection(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var section = configuration.GetSection(SectionName);
            var options = configuration.GetOptions<MongoClientOptions>(SectionName);
            services.Configure<MongoClientOptions>(section);
            services.AddSingleton(options);
            services.AddTransient<IDbContext, LibraryDbContext>();
        }

    }
}
