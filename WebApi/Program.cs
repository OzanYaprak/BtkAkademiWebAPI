using Microsoft.AspNetCore.Builder;
using WebApi.Extensions;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(); //.AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly); // AddApplicationPart kýsmý controller kýsmýný presentation kýsmýna taþýdýðýmýz için yazýldý.

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureSQLContext(builder.Configuration); // WebApi.Extensions -> ServiceExtensions
            builder.Services.ConfigureRepositoryManager(); // WebApi.Extensions -> ServiceExtensions
            builder.Services.ConfigureServiceManager(); // WebApi.Extensions -> ServiceExtensions

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwagger();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}