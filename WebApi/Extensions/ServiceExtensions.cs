using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Presentation.ActionFilters;
using Repositories.EFCore.Context;
using Repositories.Interfaces;
using Repositories.Managers;
using Services;
using Services.Interfaces;
using Services.Managers;

namespace WebApi.Extensions
{
    public static class ServiceExtensions
    {
        // Ioc Kayıtları

        public static void ConfigureSQLContext(this IServiceCollection services, IConfiguration configuration) // Eğer this eksikse, bu method bir extension method değil normal static method olarak kabul edilir.
        {
            services.AddDbContext<RepositoryContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
        }

        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerService, LoggerManager>();
        }

        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>(); // Validation Filter İçin Eklendi.
            services.AddSingleton<LogFilterAttribute>(); // Log Filter İçin Eklendi.
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin().WithExposedHeaders("X-Pagination"));
            });
        }

        public static void ConfigureDataShaper(this IServiceCollection services)
        {
            services.AddScoped<IDataShaper<BookDTO>, DataShaper<BookDTO>>();
        }

        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonOutputFormatter = config.OutputFormatters.OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();

                if (systemTextJsonOutputFormatter is not null)
                {
                    systemTextJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.btkakademi.apiroot+json");
                }

                var xmlOutpuFormatter = config.OutputFormatters.OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();

                if (xmlOutpuFormatter is not null)
                {
                    xmlOutpuFormatter.SupportedMediaTypes.Add("application/vnd.btkakademi.apiroot+xml");
                }
            });
        }
    }
}