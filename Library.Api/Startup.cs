using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Library.Api.Extensions;
using Library.Api.Services;
using Library.Domain.DbContext;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;

namespace Library.Api
{
    public class Startup
    {
        public readonly IServiceCollection _services;

        public IConfiguration Configuration { get; set; }
        //private static readonly string[] Headers = new[] { "X-Operation", "X-Resource", "X-Total-Count", "X-Pagination" };

        public Startup(IConfiguration _config)
        {
            Configuration = _config;
            //this._services = services;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMongoConnection();
            services.AddJwt();

        

            services.Configure<MvcOptions>(config =>
            {
                var newtonSoftJsonOutputFormatter = config.OutputFormatters.OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();
                if (newtonSoftJsonOutputFormatter != null)
                {
                    newtonSoftJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.marvin.hateoad+json");
                }
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddEntityServices();

            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
            .AddXmlDataContractSerializerFormatters();
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "Library Service Api" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Library Service Api");
                config.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAccessTokenValidator();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
