using Microsoft.AspNetCore.Builder;
using NLog;
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
            builder.Services.AddControllers(); //.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly); // AddApplicationPart kýsmý controller kýsmýný presentation kýsmýna taþýdýðýmýz için yazýldý.

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureSQLContext(builder.Configuration); // WebApi.Extensions -> ServiceExtensions
            builder.Services.ConfigureRepositoryManager(); // WebApi.Extensions -> ServiceExtensions
            builder.Services.ConfigureServiceManager(); // WebApi.Extensions -> ServiceExtensions
            builder.Services.ConfigureLoggerService(); // WebApi.Extensions -> ServiceExtensions

            builder.Services.AddAutoMapper(typeof(Program)); // AutoMapper

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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}