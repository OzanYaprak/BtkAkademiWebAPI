using Microsoft.AspNetCore.Builder;
using NLog;
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
            builder.Services.AddControllers(); //.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly); // AddApplicationPart k�sm� controller k�sm�n� presentation k�sm�na ta��d���m�z i�in yaz�ld�.

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureSQLContext(builder.Configuration); // WebApi.Extensions -> ServiceExtensions
            builder.Services.ConfigureRepositoryManager(); // WebApi.Extensions -> ServiceExtensions
            builder.Services.ConfigureServiceManager(); // WebApi.Extensions -> ServiceExtensions
            builder.Services.ConfigureLoggerService(); // WebApi.Extensions -> ServiceExtensions

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}