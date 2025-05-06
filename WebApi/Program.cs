using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Presentation.ActionFilters;
using Services.Interfaces;
using WebApi.Extensions;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Nlog Configuration
            LogManager.GetLogger(String.Concat(Directory.GetCurrentDirectory(), "/Nlog.config"));

            // Add services to the container.
            builder.Services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true; // ��erik pazarl��� i�in eklendi -> sadece application/json t�r�nde kabul ediliyor
                config.ReturnHttpNotAcceptable = true; // ��erik pazarl��� i�in eklendi -> sadece application/json t�r�nde kabul ediliyor
            })
                .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly) // AddApplicationPart k�sm� controller k�sm�n� presentation k�sm�na ta��d���m�z i�in yaz�ld�.
                .AddXmlDataContractSerializerFormatters() // ��erik pazarl��� i�in xml t�r�de de ��k�� kabul edilir
                .AddCustomCsvFormatter() // CsvOutputFormatter
                .AddNewtonsoftJson();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Ioc Kay�tlar�
            builder.Services.ConfigureActionFilters(); // WebApi.Extensions -> ServiceExtensions
            builder.Services.ConfigureSQLContext(builder.Configuration); // WebApi.Extensions -> ServiceExtensions
            builder.Services.ConfigureRepositoryManager(); // WebApi.Extensions -> ServiceExtensions
            builder.Services.ConfigureServiceManager(); // WebApi.Extensions -> ServiceExtensions
            builder.Services.ConfigureLoggerService(); // WebApi.Extensions -> ServiceExtensions
            builder.Services.ConfigureCors(); // WebApi.Extensions -> ServiceExtensions
            builder.Services.ConfigureDataShaper(); // WebApi.Extensions -> ServiceExtensions
            builder.Services.AddCustomMediaTypes();// WebApi.Extensions -> ServiceExtensions

            builder.Services.AddAutoMapper(typeof(Program)); // AutoMapper

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            var app = builder.Build();

            // Nlog Configuration
            var logger = app.Services.GetRequiredService<ILoggerService>();
            app.ConfigureExceptionHandler(logger); // WebApi.Extensions -> ExceptionMiddlewareExtensions

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (app.Environment.IsProduction())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy"); // ServiceExtensions -> ConfigureCors() policy name

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}