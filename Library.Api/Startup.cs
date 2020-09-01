using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Api.Extensions;
using Library.Api.Services;
using Library.Domain.DbContext;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace Library.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        private static readonly string[] Headers = new[] { "X-Operation", "X-Resource", "X-Total-Count", "X-Pagination" };

        public Startup(IConfiguration _config)
        {
            Configuration = _config;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionstring = Configuration.GetSection("DefaultConnection:ConnectionString").Value;
            string dbName = Configuration.GetSection("DefaultConnection:DataBaseName").Value;
            services.AddJwt();
            
            //services.AddAuthentication("OAuth").AddJwtBearer("OAuth", config =>
            //{

            //});

            services.AddTransient<IDbContext, LibraryDbContext>(ctx => LibraryDbContext.Create(connectionstring, dbName));
            services.Configure<MvcOptions>(config =>
            {
                var newtonSoftJsonOutputFormatter = config.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();
                if (newtonSoftJsonOutputFormatter != null)
                {
                    newtonSoftJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.marvin.hateoad+json");
                }
            });

            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
            .AddXmlDataContractSerializerFormatters();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
